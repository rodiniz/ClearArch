/**
 * Wolverine Configuration - Key Concepts and Best Practices
 * 
 * This guide explains how Wolverine replaces MassTransit in your Clean Architecture.
 * The snippets are conceptual unless noted otherwise. The template currently enables
 * local queues only; durable transports require additional Wolverine packages and
 * host-specific configuration.
 */

// 1. MESSAGE TYPES
// ===============
// Commands - Intent-based messages (CreateWorkItem, UpdateWorkItem)
// Events - Fact-based messages (WorkItemCreated, WorkItemUpdated)
// 
// In Wolverine, both are just C# records/classes with no base classes needed

// 2. HANDLERS
// ===========
// Wolverine discovers handlers by convention:
// - Methods named Handle(), HandleAsync(), Execute(), ExecuteAsync()
// - Parameter type determines what message it handles
// - Handler can return an event that gets published automatically
// 
// Example:
// public WorkItemCreatedEvent Handle(CreateWorkItemCommand command) { ... }

// 3. ROUTING & TRANSPORT
// ======================
// Local queues (in-process) - great for development
// Durable messaging options:
//   - Database transport (SQL Server, PostgreSQL)
//   - RabbitMQ
//   - Kafka
//   - AWS SQS
//   - Azure Service Bus

// To switch to RabbitMQ:
// options.UseRabbitMq("localhost")
//     .BindExchange("work-items")
//     .BindQueueToExchange("work-items.create", "work-items", "create");

// To switch to SQL Server:
// options.UseSqlServer("connection-string")
//     .AutoProvision();

// 4. ERROR HANDLING & RETRIES
// ===========================
// Configure retry policies per message type or globally
// Dead letter queues automatically capture failed messages
// 
// options.Handlers.ForMessage<CreateWorkItemCommand>()
//     .RetryOnFailure(attempts: 5)
//     .Wait(TimeSpan.FromMilliseconds(500));

// 5. SAGAS (LONG-RUNNING PROCESSES)
// ==================================
// Use Transient-scoped classes to coordinate multi-message workflows
// Return multiple events to orchestrate complex business logic
// 
// [Transient]
// public class OrderSaga {
//     public (OrderCreatedEvent, PaymentRequestedEvent) Handle(CreateOrderCommand) { ... }
// }

// 6. MIDDLEWARE
// =============
// Configure middleware for cross-cutting concerns:
// options.Handlers
//     .AddMiddleware(typeof(LoggingMiddleware))
//     .AddMiddleware(typeof(ValidationMiddleware));

// 7. TESTING
// ==========
// Use IMessageBus in unit tests
// Mock or use in-memory transport
// 
// var bus = new BusTestContext();
// await bus.SendAsync(command);
// var results = bus.CurrentMessages();

// 8. DURABILITY & RELIABILITY
// ============================
// Message box stores unsent messages locally before transport issues
// Outbox pattern support for transactional consistency
// 
// options.EnableMessageBoxing();

// 9. COMPARISON WITH MASSTRANSIT
// ===============================
// MassTransit:
//   - Interface-based (IConsumer<T>)
//   - Complex configuration
//   - Heavier framework
// 
// Wolverine:
//   - Convention-based (Method naming)
//   - Simple, fluent API
//   - Lightweight (~1MB)
//   - Faster startup
//   - Better for ASP.NET Core applications

/**
 * MIGRATION CHECKLIST FROM MASSTRANSIT
 * 
 * 1. Replace IPublishEndpoint with IMessageBus
// OLD: await publishEndpoint.Publish(new EventCreated { ... });
// NEW: await messageBus.PublishAsync(new EventCreated { ... });

 * 2. Replace IConsumer<T> with Handle methods
// OLD:
// public class EventCreatedConsumer : IConsumer<EventCreated>
// {
//     public async Task Consume(ConsumeContext<EventCreated> context) { ... }
// }
// 
// NEW:
// public class EventCreatedHandler
// {
//     public async Task Handle(EventCreated @event) { ... }
// }

 * 3. Replace container configuration
// OLD: services.AddMassTransit(x => { ... });
// NEW: builder.Host.UseWolverine(options => { ... });

 * 4. Replace transport configuration
// OLD: cfg.AddRabbitMqMessageScheduler();
// NEW: options.UseRabbitMq("localhost");
*/

/**
 * CONSUMING EVENTS FROM EXTERNAL SERVICES (e.g. Azure Service Bus)
 * =================================================================
 * 
 * Scenario: An Azure Function (or any external service) publishes a message to
 * an Azure Service Bus queue or topic, and this API needs to consume it.
 * 
 * KEY INSIGHT: The external service publishes a plain JSON message.
 * Wolverine does NOT need to be the publisher — it only needs to be the consumer.
 * You control the message contract (the C# record that maps to the JSON payload).
 */

// STEP 1 — Define the external message contract
// -----------------------------------------------
// Create a record that matches the JSON shape the external service publishes.
// Place it in Application/Messaging/Events/ (or a dedicated ExternalEvents/ folder).
//
// namespace SampleApp.Application.Messaging.Events;
//
// // Matches the JSON body that the Azure Function sends to Service Bus
// public record ExternalWorkItemApprovedEvent
// {
//     public int WorkItemId { get; init; }
//     public string ApprovedBy { get; init; } = string.Empty;
//     public DateTime ApprovedAt { get; init; }
// }


// STEP 2 — Write the handler
// ---------------------------
// Wolverine discovers it by convention (method named Handle/HandleAsync).
// No interface needed — just a plain class in a scanned assembly.
//
// namespace SampleApp.Application.Messaging.Handlers;
//
// public class ExternalWorkItemApprovedHandler
// {
//     private readonly IWorkItemRepository _repository;
//
//     public ExternalWorkItemApprovedHandler(IWorkItemRepository repository)
//         => _repository = repository;
//
//     public async Task Handle(ExternalWorkItemApprovedEvent @event, CancellationToken ct)
//     {
//         var workItem = await _repository.GetByIdAsync(@event.WorkItemId, ct);
//         if (workItem is null) return;
//
//         workItem.Approve(@event.ApprovedBy, @event.ApprovedAt);
//         await _repository.UpdateAsync(workItem, ct);
//     }
// }


// STEP 3 — Register the Azure Service Bus listener in Program.cs
// ---------------------------------------------------------------
// Add the Wolverine.AzureServiceBus NuGet package, then configure the listener.
//
// builder.Host.UseWolverine((context, options) =>
// {
//     options.Discovery.IncludeAssembly(typeof(DependencyInjection).Assembly);
//
//     var asbConnectionString = context.Configuration["AzureServiceBus:ConnectionString"];
//
//     options.UseAzureServiceBus(asbConnectionString)
//         // Listen to a queue:
//         .AddListenerForQueue("work-items-approved")
//
//         // OR listen to a topic subscription:
//         // .AddListenerForSubscription("work-items-topic", "api-subscription")
//
//         // Enable auto-provisioning (creates queue/topic if they don't exist)
//         .AutoProvision();
// });


// STEP 4 — Map the queue message to the handler (message routing)
// ---------------------------------------------------------------
// Wolverine routes by C# type. For external messages, you must tell Wolverine
// which type to deserialize the raw JSON into. Two options:

// OPTION A — Attribute on the handler (simplest)
// [WolverineHandler] // already discovered by convention, no attribute needed
// The queue name maps to the handler automatically when there is only one handler
// for that message type. Works out-of-the-box if the JSON property names match.

// OPTION B — Explicit routing in UseWolverine (recommended for external messages)
//
// options.UseAzureServiceBus(asbConnectionString)
//     .AddListenerForQueue("work-items-approved")
//     .ConfigureDeadLetterQueue("work-items-approved-dlq", dlq =>
//     {
//         dlq.MaxDeliveryCount = 5;
//     });
//
// options.ListenToAzureServiceBusQueue("work-items-approved")
//     .DefaultIncomingMessage<ExternalWorkItemApprovedEvent>() // <-- explicit mapping
//     .ProcessInline();  // or .ProcessingIsSequential() for ordered processing


// STEP 5 — appsettings.json
// -------------------------
// {
//   "AzureServiceBus": {
//     "ConnectionString": "Endpoint=sb://<namespace>.servicebus.windows.net/;SharedAccessKeyName=...;SharedAccessKey=..."
//   }
// }
//
// For production, use Managed Identity instead of a connection string:
// options.UseAzureServiceBusWithManagedIdentity("sb://<namespace>.servicebus.windows.net")


// STEP 6 — Error handling & dead-letter queue
// --------------------------------------------
// Configure retry behaviour specific to external messages, since you don't
// control the publisher and can't ask it to resend.
//
// options.Handlers.ForMessage<ExternalWorkItemApprovedEvent>()
//     .RetryOnFailure(attempts: 3)
//     .OnAnyException()
//     .Wait(TimeSpan.FromSeconds(1))
//     .ThenMoveToErrorQueue();   // sends to the DLQ after exhausting retries


// STEP 7 — Idempotency (important for external consumers)
// -------------------------------------------------------
// External services may resend messages (at-least-once delivery).
// Guard against duplicate processing with a simple idempotency check:
//
// public async Task Handle(ExternalWorkItemApprovedEvent @event, CancellationToken ct)
// {
//     var alreadyProcessed = await _repository.WasApprovalProcessedAsync(@event.WorkItemId, ct);
//     if (alreadyProcessed) return;   // idempotency guard
//
//     // ... process normally
// }


// SUMMARY — Minimal checklist for external Service Bus events
// -----------------------------------------------------------
// [ ] Add NuGet: Wolverine.AzureServiceBus
// [ ] Define a record matching the external JSON payload
// [ ] Write a handler class with a Handle(ExternalEvent) method
// [ ] Call .UseAzureServiceBus(...).AddListenerForQueue("queue-name") in UseWolverine
// [ ] Map the queue to the message type with .DefaultIncomingMessage<T>() if needed
// [ ] Add retry + DLQ policy for resilience
// [ ] Add idempotency guard in the handler

