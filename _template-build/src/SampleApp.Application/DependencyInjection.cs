using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using SampleApp.Application.Messaging.Handlers;
using SampleApp.Application.Messaging.Sagas;
using SampleApp.Application.Messaging.Middleware;

namespace SampleApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // Register Wolverine message handlers
        // These are automatically discovered by Wolverine, but explicit registration is good practice
        services.AddScoped<WorkItemEventHandler>();
        services.AddScoped<CreateWorkItemCommandHandler>();
        services.AddScoped<WorkItemSaga>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}