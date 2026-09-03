using Microsoft.EntityFrameworkCore;
using SampleApp.Domain.Entities;

namespace SampleApp.Application.Common.Interfaces;

public interface IApplicationDbContext: IDisposable
{
    DbSet<WorkItem> WorkItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}