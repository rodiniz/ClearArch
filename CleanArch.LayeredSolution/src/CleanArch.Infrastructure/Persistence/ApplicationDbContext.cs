using Microsoft.EntityFrameworkCore;
using CleanArch.Application.Common.Interfaces;
using CleanArch.Domain.Entities;

namespace CleanArch.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkItem>(builder =>
        {
            builder.HasKey(item => item.Id);
            builder.Property(item => item.Title)
                .HasMaxLength(200)
                .IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}