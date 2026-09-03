using Microsoft.EntityFrameworkCore;
using CleanArch.Application;
using CleanArch.Application.Messaging.Configuration;
using CleanArch.Infrastructure;
using CleanArch.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Wolverine;
using Wolverine.Http;

var builder = WebApplication.CreateBuilder(args);

// Configure Wolverine as the service bus (replaces MassTransit)
builder.Host.UseWolverine(options =>
{
    // Auto-discovery of message handlers and events
    options.Discovery.IncludeAssembly(typeof(CleanArch.Application.DependencyInjection).Assembly);
    
    // Apply messaging configuration
    options.AddWolverineMessaging(builder.Configuration);
});

builder.Services.AddWolverineHttp();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (ValidationException exception) when (!context.Response.HasStarted)
    {
        var errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new ValidationProblemDetails(errors));
    }
});

app.MapWolverineEndpoints();
app.MapHealthChecks("/health");

app.Run();

public partial class Program;