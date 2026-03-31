using Microsoft.EntityFrameworkCore;
using CleanArch.Application.Common.Interfaces;
using CleanArch.Domain.Entities;

namespace CleanArch.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}