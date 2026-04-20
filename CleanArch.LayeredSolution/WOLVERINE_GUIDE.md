/**
 * Wolverine Configuration - Key Concepts and Best Practices
 * 
 * This guide explains how Wolverine replaces MassTransit in your Clean Architecture
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
