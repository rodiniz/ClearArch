namespace CleanArch.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using CleanArch.Application.Common.Interfaces;
using CleanArch.Domain.Entities;

public class ApplicationDbContext: DbContext, IApplicationDbContext
{
	public DbSet<WorkItem> WorkItems => Set<WorkItem>();
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
		ChangeTracker.LazyLoadingEnabled = true;
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}