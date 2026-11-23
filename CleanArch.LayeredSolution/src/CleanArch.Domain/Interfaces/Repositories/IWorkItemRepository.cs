
using CleanArch.Domain.Entities;
using FluentResults;

namespace CleanArch.Domain.Interfaces.Repositories;

public interface IWorkItemRepository : IRepository<WorkItem>
{
	Task<Result<List<WorkItem>>> GetAll(CancellationToken cancellationToken);
}
