using CleanArch.Domain.Interfaces.Repositories;

namespace CleanArch.Application.Features.WorkItems;

public sealed record GetWorkItemsQuery;

public sealed record WorkItemDto(Guid Id, string Title, bool IsDone, DateTime CreatedAtUtc);

public sealed class GetWorkItemsQueryHandler(IWorkItemRepository repository)
{
    public async Task<IReadOnlyList<WorkItemDto>> Handle(GetWorkItemsQuery request, CancellationToken cancellationToken)
    {
        var workItemsResult = await repository.GetAll(cancellationToken);
		return workItemsResult.IsSuccess
			? workItemsResult.Value.Select(item => new WorkItemDto(item.Id, item.Title, item.IsDone, item.CreatedAtUtc)).ToList()
			: new List<WorkItemDto>();
	}
}