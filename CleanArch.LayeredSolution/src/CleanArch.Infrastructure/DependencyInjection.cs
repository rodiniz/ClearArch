using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanArch.Application.Common.Interfaces;
using CleanArch.Infrastructure.Persistence;
using CleanArch.Infrastructure.Services;

namespace CleanArch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            //#if (UsePostgres)
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Set ConnectionStrings:DefaultConnection to use the PostgreSQL provider.");
            }

            var dataSource = new Npgsql.NpgsqlDataSourceBuilder(connectionString)
                .EnableDynamicJson()
                .Build();

            options.UseNpgsql(dataSource, providerOptions =>
                providerOptions.MigrationsAssembly("CleanArch.Migrations"));
            //#endif
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}