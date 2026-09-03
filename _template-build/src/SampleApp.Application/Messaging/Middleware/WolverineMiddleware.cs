using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;

namespace SampleApp.Application.Messaging.Middleware;

/// <summary>
/// Example middleware for cross-cutting concerns
/// Wolverine middleware runs for all handlers in the chain
/// </summary>
public class ValidationMiddleware
{
    public static async Task BeforeAsync<T>(T message, IServiceProvider services, CancellationToken cancellationToken)
    {
        var validator = services.GetService<IValidator<T>>();
        if (validator is null)
        {
            return;
        }

        var result = await validator.ValidateAsync(message, cancellationToken);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }
}

/// <summary>
/// Logging middleware
/// </summary>
public class LoggingMiddleware
{
    private readonly DateTime _startTime = DateTime.UtcNow;

    public Task Before()
    {
        return Task.CompletedTask;
    }

    public Task After(Envelope envelope)
    {
        var duration = (DateTime.UtcNow - _startTime).TotalMilliseconds;
        Console.WriteLine($"[Logging] Message {envelope.Message?.GetType().Name} completed in {duration}ms");
        return Task.CompletedTask;
    }

    public Task Finally(Envelope envelope)
    {
        Console.WriteLine($"[Logging] Finalized message: {envelope.Message?.GetType().Name}");
        return Task.CompletedTask;
    }
}
