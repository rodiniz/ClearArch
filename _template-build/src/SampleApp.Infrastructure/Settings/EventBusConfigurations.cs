namespace SampleApp.Infrastructure.Settings;

using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using JasperFx.CodeGeneration;
using JasperFx.CodeGeneration.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;
using Wolverine.AzureServiceBus;

[ExcludeFromCodeCoverage]
public static class EventBusConfigurations
{
    public static IServiceCollection AddEventBusConfigurations<TMessage>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TMessage : class
    {
        var useManagedIdentity = configuration.GetValue<bool>("AzureSettings:UseManagedIdentity");
        var serviceBusNamespace = configuration["AzureSettings:ServiceBusNameSpace"];
        var connectionString = configuration["AzureSettings:ServiceBusConnectionString"];
        var queueName = configuration["AzureSettings:ServiceBusQueueName"]??"queue";
        services.AddWolverine(options =>
        {
            	options.ServiceLocationPolicy = ServiceLocationPolicy.AlwaysAllowed;
                options.CodeGeneration.TypeLoadMode = TypeLoadMode.Auto;
                //options.Discovery.IncludeAssembly(typeof(TranscriptionErrorConsumer).Assembly);
                
                if (useManagedIdentity)
                {
                    if (string.IsNullOrWhiteSpace(serviceBusNamespace))
                    {
                        throw new InvalidOperationException(
                            "AzureSettings:ServiceBusNameSpace is required when managed identity is enabled.");
                    }

                    options.UseAzureServiceBus(
                        serviceBusNamespace,
                        new DefaultAzureCredential());
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new InvalidOperationException(
                            "AzureSettings:ServiceBusConnectionString is required when managed identity is disabled.");
                    }

                    options.UseAzureServiceBus(connectionString);

                }
                
                options.UseSystemTextJsonForSerialization(json =>
                    {
                        json.PropertyNameCaseInsensitive = true;
                    });

                options.ListenToAzureServiceBusQueue(queueName).ProcessInline();
        });

        return services;
    }   

    
}