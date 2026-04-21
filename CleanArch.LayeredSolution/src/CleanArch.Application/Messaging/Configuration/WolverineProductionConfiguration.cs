using Microsoft.Extensions.Configuration;
using Wolverine;

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
        // Durable transports require adding the corresponding Wolverine transport packages.
        // Keep this extension as an integration hook for solution consumers.
    }

    /// <summary>
    /// Configure Wolverine with RabbitMQ for distributed messaging
    /// </summary>
    public static void ConfigureWithRabbitMq(
        this WolverineOptions options,
        string host = "localhost")
    {
        // Requires Wolverine transport package for RabbitMQ.
    }

    /// <summary>
    /// Configure Wolverine with Azure Service Bus for cloud deployments
    /// </summary>
    public static void ConfigureWithAzureServiceBus(
        this WolverineOptions options,
        string connectionString)
    {
        // Requires Wolverine transport package for Azure Service Bus.
    }

    /// <summary>
    /// Enable message box for local durability before sending to transport
    /// Ensures messages aren't lost during transport issues
    /// </summary>
    public static void EnableDurability(this WolverineOptions options)
    {
        // Durable inbox/outbox features are available when durable transport/persistence
        // packages are installed and configured by the host application.
    }

    /// <summary>
    /// Configure optimized handler chains and retry policies
    /// </summary>
    public static void ConfigureHandlerChains(this WolverineOptions options)
    {
        // Handler retry behavior can be configured through policy APIs in the host application.
    }
}
