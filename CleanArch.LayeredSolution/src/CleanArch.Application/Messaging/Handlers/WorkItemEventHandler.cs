using CleanArch.Application.Messaging.Events;
using Wolverine.Attributes;

namespace CleanArch.Application.Messaging.Handlers;

/// <summary>
/// Handles WorkItem domain events through Wolverine message bus
/// </summary>
public class WorkItemEventHandler
{
    /// <summary>
    /// Handles WorkItemCreatedEvent
    /// Wolverine automatically discovers and invokes this based on the message type parameter
    /// </summary>
    [Topic("work-items.events")]
    public async Task Handle(WorkItemCreatedEvent @event)
    {
        // Process the created event
        // Examples: 
        // - Send notifications
        // - Update read models
        // - Trigger downstream processes
        await Task.CompletedTask;
        
        Console.WriteLine($"WorkItem created: {event.Title} (ID: {@event.WorkItemId})");
    }

    /// <summary>
    /// Handles WorkItemUpdatedEvent
    /// </summary>
    [Topic("work-items.events")]
    public async Task Handle(WorkItemUpdatedEvent @event)
    {
        // Process the updated event
        await Task.CompletedTask;
        
        Console.WriteLine($"WorkItem updated: {event.Title} - Status: {@event.Status}");
    }
}
