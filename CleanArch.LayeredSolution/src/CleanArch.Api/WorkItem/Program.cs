using Microsoft.EntityFrameworkCore;
using CleanArch.Application;
using CleanArch.Infrastructure;
using CleanArch.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Wolverine;
using Wolverine.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(options =>
    options.Discovery.IncludeAssembly(typeof(CleanArch.Application.DependencyInjection).Assembly));

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