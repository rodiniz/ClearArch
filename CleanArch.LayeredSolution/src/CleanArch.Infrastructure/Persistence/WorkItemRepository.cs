using CleanArch.Application.Common.Interfaces;
using CleanArch.Application.Features.WorkItems;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Interfaces.Repositories;
using FluentResults;
using Microsoft.EntityFrameworkCore;


namespace CleanArch.Infrastructure.Persistence;

public class WorkItemRepository(ILog log, IApplicationDbContext dbContext) :
Repository<WorkItem>(log, dbContext), IWorkItemRepository
{
	public async Task<Result<List<WorkItem>>> GetAll(CancellationToken cancellationToken)
	{
		var workItems = await dbContext.Set<WorkItem>()
			.AsNoTracking()
			.OrderBy(item => item.CreatedAtUtc)
			.ToListAsync(cancellationToken);

		return workItems;
	}
}
