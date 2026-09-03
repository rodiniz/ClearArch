using SampleApp.Application.Features.WorkItems;
using Wolverine;
using Wolverine.Http;

namespace SampleApp.Api;

public static class WorkItemsEndpoint
{
    [WolverineGet("/api/workitems")]
    public static Task<IReadOnlyList<WorkItemDto>> Get(IMessageBus bus, CancellationToken cancellationToken)
    {
        return bus.InvokeAsync<IReadOnlyList<WorkItemDto>>(new GetWorkItemsQuery(), cancellationToken);
    }

    [WolverinePost("/api/workitems")]
    public static async Task<WorkItemCreationResponse> Post(CreateWorkItemCommand command, IMessageBus bus, CancellationToken cancellationToken)
    {
        var item = await bus.InvokeAsync<WorkItemDto>(command, cancellationToken);
        return new WorkItemCreationResponse(item.Id);
    }

    [WolverinePost("/api/workitems/with-event")]
    public static async Task<(WorkItemCreationResponse, WorkItemCreated)> PostWithEvent(
        CreateWorkItemCommand command,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var item = await bus.InvokeAsync<WorkItemDto>(command, cancellationToken);
        return (new WorkItemCreationResponse(item.Id), new WorkItemCreated(item.Id));
    }
}

