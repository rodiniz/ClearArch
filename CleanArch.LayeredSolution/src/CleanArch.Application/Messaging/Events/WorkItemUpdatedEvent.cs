namespace CleanArch.Application.Messaging.Events;

public record WorkItemUpdatedEvent
{
    public int WorkItemId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime UpdatedAt { get; init; }
}
