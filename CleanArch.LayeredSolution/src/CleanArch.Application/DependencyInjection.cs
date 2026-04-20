using Microsoft.Extensions.DependencyInjection;
using CleanArch.Application.Messaging.Handlers;
using CleanArch.Application.Messaging.Sagas;

namespace CleanArch.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // Register Wolverine message handlers
        // These are automatically discovered by Wolverine, but explicit registration is good practice
        services.AddScoped<WorkItemEventHandler>();
        services.AddScoped<CreateWorkItemCommandHandler>();
        services.AddScoped<WorkItemSaga>();

        return services;
    }
}