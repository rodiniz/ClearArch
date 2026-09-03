using Wolverine.Attributes;

namespace SampleApp.Application.Messaging.Commands;

/// <summary>
/// Command to create a new work item.
/// The [Topic] attribute routes this to a specific queue.
/// </summary>
[Topic("work-items.create")]
public record CreateWorkItemCommand
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
}
