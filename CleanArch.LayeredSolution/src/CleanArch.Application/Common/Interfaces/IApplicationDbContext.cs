using Microsoft.EntityFrameworkCore;
using CleanArch.Domain.Entities;

namespace CleanArch.Application.Common.Interfaces;

public interface IApplicationDbContext: IDisposable
{
    DbSet<WorkItem> WorkItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}