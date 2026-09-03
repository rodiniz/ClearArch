using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CleanArch.Infrastructure.Persistence;

namespace CleanArch.Migrations;

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}