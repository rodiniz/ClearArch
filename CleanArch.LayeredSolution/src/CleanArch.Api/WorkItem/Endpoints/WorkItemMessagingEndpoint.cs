using CleanArch.Application.Messaging.Commands;
using Wolverine;

namespace CleanArch.Api.WorkItem.Endpoints;

/// <summary>
/// HTTP endpoint that demonstrates sending messages through Wolverine
/// </summary>
public class WorkItemMessagingEndpoint
{
    /// <summary>
    /// POST /workitems/messaging
    /// Creates a work item by sending a CreateWorkItemCommand through the message bus
    /// </summary>
    public async Task<CreateWorkItemResponse> CreateWorkItemAsync(
        CreateWorkItemRequest request,
        IMessageBus messageBus)
    {
        var command = new CreateWorkItemCommand
        {
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate
        };

        // Send command through Wolverine message bus
        // This will route to the appropriate handler
        await messageBus.SendAsync(command);

        return new CreateWorkItemResponse
        {
            Message = "Work item creation initiated",
            Status = "Queued"
        };
    }

    /// <summary>
    /// POST /workitems/publish-event
    /// Demonstrates publishing an event through Wolverine
    /// </summary>
    public async Task<PublishEventResponse> PublishEventAsync(
        PublishEventRequest request,
        IMessageBus messageBus)
    {
        // Create and publish event
        // In real applications, events are typically published from within handlers
        // This is for demonstration purposes

        return new PublishEventResponse
        {
            Message = "Event published successfully",
            Status = "Published"
        };
    }
}

public record CreateWorkItemRequest
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
}

public record CreateWorkItemResponse
{
    public string Message { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}

public record PublishEventRequest
{
    public string EventType { get; init; } = string.Empty;
    public string Data { get; init; } = string.Empty;
}

public record PublishEventResponse
{
    public string Message { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}
