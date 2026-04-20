using CleanArch.Application.Messaging.Commands;
using CleanArch.Application.Messaging.Events;
using Wolverine;

namespace CleanArch.Application.Messaging.Handlers;

/// <summary>
/// Advanced handler showcasing Wolverine features like:
/// - Dependency injection
/// - Multiple return values (cascading messages)
/// - Async operations
/// - Error handling attributes
/// </summary>
public class AdvancedWorkItemHandler
{
    private readonly IMessageBus _messageBus;

    public AdvancedWorkItemHandler(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    /// <summary>
    /// Handler that returns multiple messages to cascade
    /// All returned messages are automatically published
    /// </summary>
    public (WorkItemCreatedEvent, WorkItemNotificationEvent) Handle(CreateWorkItemCommand command)
    {
        ValidateCommand(command);

        var workItemId = new Random().Next(1, 1000);

        var createdEvent = new WorkItemCreatedEvent
        {
            WorkItemId = workItemId,
            Title = command.Title,
            Description = command.Description,
            CreatedAt = DateTime.UtcNow
        };

        var notificationEvent = new WorkItemNotificationEvent
        {
            WorkItemId = workItemId,
            Message = $"New work item created: {command.Title}"
        };

        return (createdEvent, notificationEvent);
    }

    /// <summary>
    /// Handler with complex async operations
    /// </summary>
    public async Task HandleAsync(WorkItemCreatedEvent @event)
    {
        // Simulate async operations (database, external API calls, etc.)
        await Task.Delay(100);

        Console.WriteLine($"Processed WorkItemCreatedEvent: {@event.WorkItemId}");

        // You can publish additional messages by injecting IMessageBus
        // await _messageBus.PublishAsync(new SomeOtherEvent { ... });
    }

    private void ValidateCommand(CreateWorkItemCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
            throw new ArgumentException("Title is required");

        if (command.DueDate < DateTime.UtcNow)
            throw new ArgumentException("Due date must be in the future");
    }
}

/// <summary>
/// Event for notifications
/// </summary>
public record WorkItemNotificationEvent
{
    public int WorkItemId { get; init; }
    public string Message { get; init; } = string.Empty;
}
