using CleanArch.Application.Messaging.Commands;
using CleanArch.Application.Messaging.Events;

namespace CleanArch.Application.Messaging.Handlers;

/// <summary>
/// Command handler for creating work items
/// Wolverine routes commands to handlers by convention (ICommandHandler<T>) or
/// by method naming (Handle/Execute) and message type parameters
/// </summary>
public class CreateWorkItemCommandHandler
{
    /// <summary>
    /// Handles the CreateWorkItemCommand and returns an event to be published
    /// Wolverine automatically publishes the returned event
    /// </summary>
    public WorkItemCreatedEvent Handle(CreateWorkItemCommand command)
    {
        // Simulate work item creation
        var workItemId = new Random().Next(1, 1000);
        
        // Return event that Wolverine will publish
        return new WorkItemCreatedEvent
        {
            WorkItemId = workItemId,
            Title = command.Title,
            Description = command.Description,
            CreatedAt = DateTime.UtcNow
        };
    }
}
