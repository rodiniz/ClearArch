using CleanArch.Application.Messaging.Commands;
using CleanArch.Application.Messaging.Events;
using Wolverine.Attributes;

namespace CleanArch.Application.Messaging.Sagas;

/// <summary>
/// Long-running process (saga) that coordinates multiple messages and business logic
/// Useful for complex workflows like order processing, approval flows, etc.
/// </summary>
[Transient]
public class WorkItemSaga
{
    public int WorkItemId { get; set; }
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Start the saga with a command
    /// </summary>
    public (WorkItemCreatedEvent, WorkItemNotificationEvent) Handle(CreateWorkItemCommand command)
    {
        WorkItemId = new Random().Next(1, 1000);
        Title = command.Title;

        var createdEvent = new WorkItemCreatedEvent
        {
            WorkItemId = WorkItemId,
            Title = Title,
            Description = command.Description,
            CreatedAt = DateTime.UtcNow
        };

        var notificationEvent = new WorkItemNotificationEvent
        {
            WorkItemId = WorkItemId,
            Message = $"Work item '{Title}' has been created successfully"
        };

        return (createdEvent, notificationEvent);
    }
}

/// <summary>
/// Notification event emitted during saga orchestration
/// </summary>
public record WorkItemNotificationEvent
{
    public int WorkItemId { get; init; }
    public string Message { get; init; } = string.Empty;
}
