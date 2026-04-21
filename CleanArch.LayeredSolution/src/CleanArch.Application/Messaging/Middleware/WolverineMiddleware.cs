using Wolverine;

namespace CleanArch.Application.Messaging.Middleware;

/// <summary>
/// Example middleware for cross-cutting concerns
/// Wolverine middleware runs for all handlers in the chain
/// </summary>
public class ValidationMiddleware
{
    public Task Before(Envelope envelope)
    {
        Console.WriteLine($"[Validation] Processing message: {envelope.Message?.GetType().Name}");
        return Task.CompletedTask;
    }

    public Task After(Envelope envelope)
    {
        Console.WriteLine($"[Validation] Completed message: {envelope.Message?.GetType().Name}");
        return Task.CompletedTask;
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
