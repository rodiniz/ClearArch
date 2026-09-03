using SampleApp.Application;
using SampleApp.Application.Messaging.Configuration;
using SampleApp.Infrastructure;
using Scalar.AspNetCore;
using Wolverine;
using Wolverine.Http;
using SampleApp.Api;

var builder = WebApplication.CreateBuilder(args);

// Configure Wolverine as the service bus (replaces MassTransit)
builder.Host.UseWolverine(options =>
{
    // Auto-discovery of message handlers and events
    options.Discovery.IncludeAssembly(typeof(SampleApp.Application.DependencyInjection).Assembly);
    
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

app.UseExceptionMiddleWare();

app.MapWolverineEndpoints();
app.MapHealthChecks("/health");

app.Run();

public partial class Program;