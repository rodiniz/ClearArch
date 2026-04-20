using Microsoft.Extensions.Configuration;
using Wolverine;
using Wolverine.Runtime.Routing;

namespace CleanArch.Application.Messaging.Configuration;

/// <summary>
/// Wolverine configuration for the application
/// Replaces MassTransit with a modern, lightweight service bus
/// </summary>
public static class WolverineConfiguration
{
    /// <summary>
    /// Configure Wolverine with best practices for a Clean Architecture application
    /// </summary>
    public static void AddWolverineMessaging(this WolverineOptions options, IConfiguration configuration)
    {
        // Local transport for in-process messaging (useful for initial development)
        // For production, switch to durable messaging (SQL Server, PostgreSQL, RabbitMQ, etc.)
        options.UseLocalQueue("work-items.create")
            .Sequential();

        options.UseLocalQueue("work-items.events")
            .Sequential();

        // Dead letter handling - messages that fail processing
        options.DeadLetterQueue.IncludeAllExceptionTypes = true;

        // Automatic message discovery
        // Scans the assembly for message classes and handlers
        options.Discovery
            .IncludeAssembly(typeof(WolverineConfiguration).Assembly)
            .DisableConventionalDiscovery(type => type.Namespace?.Contains("obj") ?? false);

        // Configure message routing
        ConfigureMessageRouting(options);

        // Configure error handling and retries
        ConfigureErrorHandling(options);
    }

    private static void ConfigureMessageRouting(WolverineOptions options)
    {
        // Route specific command types to specific queues
        // This is useful for scaling specific message types
        options.PublishAllMessages()
            .ToLocalQueue("work-items.create");

        options.PublishAllEvents()
            .ToLocalQueue("work-items.events");
    }

    private static void ConfigureErrorHandling(WolverineOptions options)
    {
        // Configure retry policy for failed messages
        options.Handlers
            .ConfigureConventionalHandlers()
            .RetryOnFailure(attempts: 3)
            .OnAnyException()
            .Wait(TimeSpan.FromMilliseconds(100));

        // Customize specific handler chains
        // options.Handlers.ForMessage<CreateWorkItemCommand>()
        //     .RetryOnFailure(attempts: 5);
    }
}
