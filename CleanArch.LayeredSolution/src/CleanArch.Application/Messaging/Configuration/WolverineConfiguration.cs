using Microsoft.Extensions.Configuration;
using CleanArch.Application.Messaging.Commands;
using CleanArch.Application.Messaging.Events;
using CleanArch.Application.Messaging.Sagas;
using CleanArch.Application.Messaging.Middleware;
using Wolverine;

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
        // Automatic message discovery
        // Scans the assembly for message classes and handlers
        options.Discovery
            .IncludeAssembly(typeof(WolverineConfiguration).Assembly);

        options.Policies.AddMiddleware<ValidationMiddleware>();

    }
}
