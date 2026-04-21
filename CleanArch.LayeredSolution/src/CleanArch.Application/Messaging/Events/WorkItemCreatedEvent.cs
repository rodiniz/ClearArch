using Wolverine.Attributes;

namespace CleanArch.Application.Messaging.Events;

[Topic("work-items.events")]
public record WorkItemCreatedEvent
{
    public int WorkItemId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
