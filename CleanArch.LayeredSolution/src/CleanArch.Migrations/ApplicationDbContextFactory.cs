using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CleanArch.Infrastructure.Persistence;

namespace CleanArch.Migrations;

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlite(
            "Data Source=CleanArch.db",
            builder => builder.MigrationsAssembly(typeof(AssemblyMarker).Assembly.GetName().Name));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}