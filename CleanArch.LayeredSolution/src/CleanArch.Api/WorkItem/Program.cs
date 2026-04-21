using Microsoft.EntityFrameworkCore;
using CleanArch.Application;
using CleanArch.Application.Messaging.Configuration;
using CleanArch.Infrastructure;
using CleanArch.Infrastructure.Persistence;
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

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapWolverineEndpoints();
app.MapHealthChecks("/health");

app.Run();

public partial class Program;