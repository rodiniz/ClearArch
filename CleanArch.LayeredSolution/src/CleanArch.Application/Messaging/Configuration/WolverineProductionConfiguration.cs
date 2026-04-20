using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Wolverine;
using Wolverine.Runtime;

namespace CleanArch.Application.Messaging.Configuration;

/// <summary>
/// Production-ready Wolverine configuration with multiple transport options
/// </summary>
public static class WolverineProductionConfiguration
{
    /// <summary>
    /// Configure Wolverine with database-backed durable messaging (SQL Server)
    /// Use this for production environments requiring message durability
    /// </summary>
    public static void ConfigureWithSqlServer(
        this WolverineOptions options,
        string connectionString)
    {
        options.UseSqlServerPersistence(connectionString)
            .AutoProvision();

        options.Services.AddLogging(builder =>
            builder.SetMinimumLevel(LogLevel.Information));
    }

    /// <summary>
    /// Configure Wolverine with RabbitMQ for distributed messaging
    /// </summary>
    public static void ConfigureWithRabbitMq(
        this WolverineOptions options,
        string host = "localhost")
    {
        options.UseRabbitMq(host)
            .BindExchange("work-items")
            .BindQueueToExchange("work-items.create", "work-items", "create")
            .BindQueueToExchange("work-items.events", "work-items", "events");

        options.DeadLetterQueue.IncludeAllExceptionTypes = true;
    }

    /// <summary>
    /// Configure Wolverine with Azure Service Bus for cloud deployments
    /// </summary>
    public static void ConfigureWithAzureServiceBus(
        this WolverineOptions options,
        string connectionString)
    {
        options.UseAzureServiceBus(connectionString)
            .Endpoint(new Uri("sb://work-items/"))
            .ConfigureEndpoint(e =>
            {
                e.SubscriptionName = "work-items-consumer";
            });
    }

    /// <summary>
    /// Enable message box for local durability before sending to transport
    /// Ensures messages aren't lost during transport issues
    /// </summary>
    public static void EnableDurability(this WolverineOptions options)
    {
        options.EnableMessageBoxing();
    }

    /// <summary>
    /// Configure optimized handler chains and retry policies
    /// </summary>
    public static void ConfigureHandlerChains(this WolverineOptions options)
    {
        options.Handlers
            .ConfigureConventionalHandlers()
            .RetryOnFailure(attempts: 3)
            .OnAnyException()
            .Wait(TimeSpan.FromMilliseconds(100));

        // Custom retry policy for specific message types
        // Uncomment and customize as needed:
        // options.Handlers.ForMessage<CriticalCommand>()
        //     .RetryOnFailure(attempts: 5)
        //     .OnException<TimeoutException>()
        //     .Wait(TimeSpan.FromSeconds(1))
        //     .Then()
        //     .Wait(TimeSpan.FromSeconds(2));
    }
}
