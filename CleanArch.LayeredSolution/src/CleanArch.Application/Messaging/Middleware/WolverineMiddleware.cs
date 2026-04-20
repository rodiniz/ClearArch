using Wolverine.Runtime.Handlers;

namespace CleanArch.Application.Messaging.Middleware;

/// <summary>
/// Example middleware for cross-cutting concerns
/// Wolverine middleware runs for all handlers in the chain
/// </summary>
public class ValidationMiddleware
{
    public async Task Handle(
        IInvokeChain chain,
        MessageContext context)
    {
        Console.WriteLine($"[Validation] Processing message: {context.Message?.GetType().Name}");
        
        // Pre-processing logic here
        // You can validate the message, check permissions, etc.

        // Continue to next middleware/handler
        await chain.InvokeAsync();

        // Post-processing logic
        Console.WriteLine($"[Validation] Completed message: {context.Message?.GetType().Name}");
    }
}

/// <summary>
/// Logging middleware
/// </summary>
public class LoggingMiddleware
{
    public async Task Handle(
        IInvokeChain chain,
        MessageContext context)
    {
        var startTime = DateTime.UtcNow;
        
        try
        {
            await chain.InvokeAsync();
            var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
            Console.WriteLine($"[Logging] Message {context.Message?.GetType().Name} completed in {duration}ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Logging] Message {context.Message?.GetType().Name} failed: {ex.Message}");
            throw;
        }
    }
}
