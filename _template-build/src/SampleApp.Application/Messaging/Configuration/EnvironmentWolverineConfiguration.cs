using Microsoft.Extensions.Configuration;
using Wolverine;

namespace SampleApp.Application.Messaging.Configuration;

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
        return options;
    }

    private static WolverineOptions ConfigureProduction(
        this WolverineOptions options,
        IConfiguration configuration)
    {
        return options;
    }

    private static WolverineOptions ConfigureStaging(
        this WolverineOptions options,
        IConfiguration configuration)
    {
        return options;
    }
}
