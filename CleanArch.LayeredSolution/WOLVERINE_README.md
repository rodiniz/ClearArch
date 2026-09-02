# Wolverine Configuration Guide

## Overview

This project has been configured to use **Wolverine** as the service bus and messaging framework, replacing MassTransit. The template enables Wolverine's local in-process queues by default. SQL Server, RabbitMQ, and Azure Service Bus examples below are optional integrations and require their corresponding Wolverine transport packages plus host-specific wiring.

## Project Structure

```
CleanArch.Application/
├── Messaging/
│   ├── Commands/          # Command definitions
│   │   └── CreateWorkItemCommand.cs
│   ├── Events/            # Event definitions
│   │   ├── WorkItemCreatedEvent.cs
│   │   └── WorkItemUpdatedEvent.cs
│   ├── Handlers/          # Message handlers
│   │   ├── WorkItemEventHandler.cs
│   │   ├── CreateWorkItemCommandHandler.cs
│   │   └── AdvancedWorkItemHandler.cs
│   ├── Sagas/             # Long-running processes
│   │   └── WorkItemSaga.cs
│   ├── Middleware/        # Cross-cutting concerns
│   │   └── WolverineMiddleware.cs
│   └── Configuration/     # Wolverine setup
│       ├── WolverineConfiguration.cs
│       └── WolverineProductionConfiguration.cs
```

## Quick Start

### 1. Basic Usage - Sending Commands

```csharp
// Inject IMessageBus into your handler or controller
public class YourController
{
    private readonly IMessageBus _messageBus;
    
    public YourController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }
    
    public async Task SendCommand()
    {
        var command = new CreateWorkItemCommand
        {
            Title = "My Task",
            Description = "Task description",
            DueDate = DateTime.UtcNow.AddDays(1)
        };
        
        // Send the command - Wolverine routes it to the appropriate handler
        await _messageBus.SendAsync(command);
    }
}
```

### 2. Publishing Events

```csharp
// Handler automatically publishes returned events
public class CreateWorkItemCommandHandler
{
    public WorkItemCreatedEvent Handle(CreateWorkItemCommand command)
    {
        // Business logic here
        
        return new WorkItemCreatedEvent
        {
            WorkItemId = 123,
            Title = command.Title,
            CreatedAt = DateTime.UtcNow
        };
    }
}
```

### 3. Handling Events

```csharp
public class WorkItemEventHandler
{
    public async Task Handle(WorkItemCreatedEvent @event)
    {
        // React to the event
        Console.WriteLine($"Work item created: {@event.Title}");
    }
}
```

## Core Concepts

### Messages

**Commands**: Intent-based messages representing actions to perform
```csharp
public record CreateWorkItemCommand
{
    public string Title { get; init; }
    public string Description { get; init; }
}
```

**Events**: Fact-based messages representing something that happened
```csharp
public record WorkItemCreatedEvent
{
    public int WorkItemId { get; init; }
    public string Title { get; init; }
    public DateTime CreatedAt { get; init; }
}
```

### Handlers

Wolverine discovers handlers by convention:

```csharp
// Method naming convention (Handle, HandleAsync, Execute, ExecuteAsync)
public class MyHandler
{
    public void Handle(MyMessage message) { }
    public async Task HandleAsync(MyMessage message) { }
    public MyEvent Execute(MyCommand command) { }
}
```

**Handler Return Values**:
- `void/Task`: Message processed, nothing published
- `Event`: Single event published
- `(Event1, Event2, ...)`: Multiple events published (cascading)

### Transport Options

#### Development (Default)
Uses in-process local queues:
```csharp
options.UseLocalQueue("work-items.create").Sequential();
```

#### Production - SQL Server (optional integration)
For durable messaging:
```csharp
options.UseSqlServerPersistence(connectionString).AutoProvision();
```

#### Production - RabbitMQ (optional integration)
For distributed messaging:
```csharp
options.UseRabbitMq("localhost")
    .BindExchange("work-items")
    .BindQueueToExchange("work-items.create", "work-items", "create");
```

#### Production - Azure Service Bus (optional integration)
For cloud deployments:
```csharp
options.UseAzureServiceBus(connectionString);
```

## Configuration Examples

### Enable Message Durability (SQL Server)

In `Program.cs`:
```csharp
builder.Host.UseWolverine((context, options) =>
{
    options.UseSqlServerPersistence(context.Configuration.GetConnectionString("WolverineDb"))
        .AutoProvision();
});
```

In `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "WolverineDb": "Server=.;Database=Wolverine;Integrated Security=true;"
  }
}
```

### Configure Retry Policies

```csharp
options.Handlers
    .ForMessage<CriticalCommand>()
    .RetryOnFailure(attempts: 5)
    .OnException<TimeoutException>()
    .Wait(TimeSpan.FromSeconds(1));
```

### Add Middleware

```csharp
options.Handlers
    .AddMiddleware(typeof(LoggingMiddleware))
    .AddMiddleware(typeof(ValidationMiddleware));
```

## Advanced Patterns

### Sagas (Long-Running Processes)

Coordinate complex workflows across multiple messages:

```csharp
[Transient]
public class OrderProcessingSaga
{
    public (OrderCreatedEvent, PaymentRequestedEvent) Handle(CreateOrderCommand command)
    {
        // Orchestrate creation and payment flow
        return (createdEvent, paymentEvent);
    }
}
```

### Cascading Messages

Return multiple messages from a handler - all are published:

```csharp
public (Event1, Event2, Event3) Handle(MyCommand command)
{
    return (event1, event2, event3);
}
```

### Dead Letter Queue

Failed messages automatically go to the dead letter queue:
```csharp
options.DeadLetterQueue.IncludeAllExceptionTypes = true;
```

## Testing

### Unit Test Handlers

```csharp
[Fact]
public void CreateWorkItemHandler_Returns_CreatedEvent()
{
    var handler = new CreateWorkItemCommandHandler();
    var command = new CreateWorkItemCommand { Title = "Test" };
    
    var result = handler.Handle(command);
    
    result.Title.Should().Be("Test");
}
```

This template uses xUnit and FluentAssertions. Add `using Xunit;` and `using FluentAssertions;` to a real test file.

### Integration Test with Message Bus

```csharp
[Fact]
public async Task MessageBus_Routes_Command_To_Handler()
{
    var host = await Host.CreateDefaultBuilder()
        .UseWolverine()
        .StartAsync();
    
    var bus = host.Services.GetRequiredService<IMessageBus>();
    var command = new CreateWorkItemCommand { Title = "Test" };
    
    await bus.SendAsync(command);
    
    // Add an assertion that observes the handler result or persisted state.
}
```

## Dependency Injection

Handlers automatically get dependencies injected:

```csharp
public class WorkItemHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<WorkItemHandler> _logger;
    
    // Constructor injection works automatically when the handler runs inside the Wolverine host.
    public WorkItemHandler(IApplicationDbContext dbContext, ILogger<WorkItemHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task Handle(WorkItemCreatedEvent @event)
    {
        _logger.LogInformation($"Saving work item: {@event.Title}");
        // Use injected dependencies...
    }
}
```

## Migration from MassTransit

### 1. Replace Consumer with Handler

**MassTransit**:
```csharp
public class EventCreatedConsumer : IConsumer<EventCreated>
{
    public async Task Consume(ConsumeContext<EventCreated> context)
    {
        // Handle event
    }
}
```

**Wolverine**:
```csharp
public class EventCreatedHandler
{
    public async Task Handle(EventCreated @event)
    {
        // Handle event
    }
}
```

### 2. Replace Publish Endpoint

**MassTransit**:
```csharp
await publishEndpoint.Publish(new EventCreated { ... });
```

**Wolverine**:
```csharp
await messageBus.PublishAsync(new EventCreated { ... });
```

### 3. Update Container Configuration

**MassTransit**:
```csharp
services.AddMassTransit(x =>
{
    x.AddConsumer<EventCreatedConsumer>();
    x.UsingRabbitMq();
});
```

**Wolverine**:
```csharp
builder.Host.UseWolverine((context, options) =>
{
    options.Discovery.IncludeAssembly(typeof(EventCreatedHandler).Assembly);
    options.UseRabbitMq("localhost");
});
```

## Best Practices

1. **Message Design**
   - Keep messages small and focused
   - Use immutable records (`record` class)
   - Include correlation IDs for tracing

2. **Handler Design**
   - Keep handlers stateless
   - Follow Single Responsibility Principle
   - Use dependency injection for external services

3. **Error Handling**
   - Configure appropriate retry policies
   - Use dead letter queues
   - Log exceptions with full context

4. **Performance**
   - Process large volumes with batching
   - Use sequential processing for order-dependent messages
   - Monitor handler execution times

5. **Testing**
   - Test handlers in isolation
   - Test integration with real transport in CI/CD
   - Use test fixtures for complex scenarios

## Debugging

Enable detailed logging:

```json
{
  "Logging": {
    "LogLevel": {
      "Wolverine": "Debug"
    }
  }
}
```

Monitor message processing:
```csharp
var diagnostics = host.Services.GetRequiredService<IWolverineRuntime>();
var stats = diagnostics.Session.Aggregate;
Console.WriteLine($"Processed: {stats.ProcessedMessages}");
Console.WriteLine($"Failed: {stats.FailedMessages}");
```

## Resources

- [Wolverine Documentation](https://jeremydmiller.com/wolverine/)
- [GitHub Repository](https://github.com/JasperFx/wolverine)
- [Examples and Recipes](https://jeremydmiller.com/wolverine/guide/)

## Additional Files

- `WOLVERINE_GUIDE.md` - Comprehensive guide and MassTransit comparison
- `WolverineConfiguration.cs` - Development configuration
- `WolverineProductionConfiguration.cs` - Production configuration examples
- `WorkItemMessagingEndpoint.cs` - HTTP endpoint examples
