using Microsoft.Extensions.Configuration;
using SampleApp.Application.Messaging.Commands;
using SampleApp.Application.Messaging.Events;
using SampleApp.Application.Messaging.Sagas;
using SampleApp.Application.Messaging.Middleware;
using Wolverine;

namespace SampleApp.Application.Messaging.Configuration;

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
        // Automatic message discovery
        // Scans the assembly for message classes and handlers
        options.Discovery
            .IncludeAssembly(typeof(WolverineConfiguration).Assembly);

        options.Policies.AddMiddleware<ValidationMiddleware>();

    }
}
