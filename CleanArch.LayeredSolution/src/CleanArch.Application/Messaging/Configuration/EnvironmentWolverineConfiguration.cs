using Microsoft.Extensions.Configuration;
using Wolverine;

namespace CleanArch.Application.Messaging.Configuration;

/// <summary>
/// Environment-specific Wolverine configuration
/// Loads appropriate settings based on the hosting environment
/// </summary>
public static class EnvironmentWolverineConfiguration
{
    public static WolverineOptions ConfigureByEnvironment(
        this WolverineOptions options,
        IConfiguration configuration,
        string environment)
    {
        return environment switch
        {
            "Development" => options.ConfigureDevelopment(),
            "Production" => options.ConfigureProduction(configuration),
            "Staging" => options.ConfigureStaging(configuration),
            _ => options.ConfigureDevelopment()
        };
    }

    private static WolverineOptions ConfigureDevelopment(this WolverineOptions options)
    {
        // Use in-process queues for fast development feedback
        options.UseLocalQueue("work-items.create").Sequential();
        options.UseLocalQueue("work-items.events").Sequential();

        // Enable detailed logging
        options.Handlers
            .ConfigureConventionalHandlers()
            .RetryOnFailure(attempts: 1); // Quick feedback in dev

        return options;
    }

    private static WolverineOptions ConfigureProduction(
        this WolverineOptions options,
        IConfiguration configuration)
    {
        // Use durable SQL Server transport
        var connectionString = configuration.GetConnectionString("WolverineDb")
            ?? throw new InvalidOperationException("WolverineDb connection string not found");

        options.UseSqlServerPersistence(connectionString)
            .AutoProvision();

        // Production-grade retry policy
        options.Handlers
            .ConfigureConventionalHandlers()
            .RetryOnFailure(attempts: 5)
            .OnAnyException()
            .Wait(TimeSpan.FromMilliseconds(500))
            .Then()
            .Wait(TimeSpan.FromSeconds(1))
            .Then()
            .Wait(TimeSpan.FromSeconds(2));

        // Enable message boxing for local durability
        options.EnableMessageBoxing();

        // Dead letter handling
        options.DeadLetterQueue.IncludeAllExceptionTypes = true;

        return options;
    }

    private static WolverineOptions ConfigureStaging(
        this WolverineOptions options,
        IConfiguration configuration)
    {
        // Similar to production but with different logging level
        var connectionString = configuration.GetConnectionString("WolverineDb")
            ?? throw new InvalidOperationException("WolverineDb connection string not found");

        options.UseSqlServerPersistence(connectionString)
            .AutoProvision();

        options.Handlers
            .ConfigureConventionalHandlers()
            .RetryOnFailure(attempts: 3)
            .OnAnyException()
            .Wait(TimeSpan.FromMilliseconds(500));

        return options;
    }
}
