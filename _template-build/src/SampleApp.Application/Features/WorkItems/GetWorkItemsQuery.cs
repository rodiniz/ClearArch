using Microsoft.EntityFrameworkCore;
using SampleApp.Application.Common.Interfaces;

namespace SampleApp.Application.Features.WorkItems;

public sealed record GetWorkItemsQuery;

public sealed record WorkItemDto(Guid Id, string Title, bool IsDone, DateTime CreatedAtUtc);

public sealed class GetWorkItemsQueryHandler(IApplicationDbContext context)
{
    public async Task<IReadOnlyList<WorkItemDto>> Handle(GetWorkItemsQuery request, CancellationToken cancellationToken)
    {
        return await context.WorkItems
            .AsNoTracking()
            .OrderBy(item => item.CreatedAtUtc)
            .Select(item => new WorkItemDto(item.Id, item.Title, item.IsDone, item.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}