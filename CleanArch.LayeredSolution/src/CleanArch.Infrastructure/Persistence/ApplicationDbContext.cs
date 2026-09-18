namespace CleanArch.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using CleanArch.Application.Common.Interfaces;
using CleanArch.Domain.Entities;
using CleanArch.Infrastructure.Settings;
using Npgsql;

public class ApplicationDbContext: DbContext, IApplicationDbContext
{
	private readonly NpgsqlSettings configuration;
	public DbSet<WorkItem> WorkItems => Set<WorkItem>();
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
		NpgsqlSettings configuration) : base(options)
	{
		this.configuration = configuration;
	
		ChangeTracker.LazyLoadingEnabled = true;
	}
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.ConnectionString)
			.EnableDynamicJson()
			.Build();

		optionsBuilder.UseNpgsql(dataSourceBuilder, b =>
		{			
			b.MigrationsAssembly("CleanArch.Migrations");			
		});
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}