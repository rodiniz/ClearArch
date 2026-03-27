using Microsoft.EntityFrameworkCore;
using CleanArch.Application.Common.Interfaces;

namespace CleanArch.Application.Features.WorkItems;

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