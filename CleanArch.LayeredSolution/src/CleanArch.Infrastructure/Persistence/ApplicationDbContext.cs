namespace CleanArch.Infrastructure.Persistence;

using CleanArch.Application.Common.Interfaces;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Interfaces.Repositories.Common;

using Microsoft.EntityFrameworkCore;


public class ApplicationDbContext: DbContext, IApplicationDbContext
{
	public DbSet<WorkItem> WorkItems { get; set; }
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
		ChangeTracker.LazyLoadingEnabled = true;
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

	IDbSet<T> IApplicationDbContext.Set<T>() where T : class
	{
		return typeof(T) switch
		{
			var _ when typeof(T).Equals(typeof(WorkItem)) =>
				(IDbSet<T>)new DbSetWrapper<WorkItem>(WorkItems, this),
			
			_ => throw new NotSupportedException($"Type {typeof(T)} is not supported."),
		};
	}
}