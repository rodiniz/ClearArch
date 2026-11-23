using CleanArch.Application;
using CleanArch.Application.Features.WorkItems;
using CleanArch.Infrastructure;
using Scalar.AspNetCore;
using Wolverine;
using CleanArch.Api;
using JasperFx;

var builder = WebApplication.CreateBuilder(args);

builder.AddWolverine();

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

app.MapGet("/api/workitems", async (IMessageBus bus, CancellationToken cancellationToken) =>
    await bus.InvokeAsync<IReadOnlyList<WorkItemDto>>(new GetWorkItemsQuery(), cancellationToken));

app.MapPost("/api/workitems", async (
    CreateWorkItemCommand command,
    IMessageBus bus,
    CancellationToken cancellationToken) =>
{
    var item = await bus.InvokeAsync<WorkItemDto>(command, cancellationToken);
    return Results.Created($"/api/workitems/{item.Id}", item);
});

app.MapPost("/api/workitems/with-event", async (
    CreateWorkItemCommand command,
    IMessageBus bus,
    CancellationToken cancellationToken) =>
{
    var item = await bus.InvokeAsync<WorkItemDto>(command, cancellationToken);
    await bus.PublishAsync(new WorkItemCreated(item.Id));
    return Results.Created($"/api/workitems/{item.Id}", item);
});

app.MapHealthChecks("/health");

return await app.RunJasperFxCommands(args);

public partial class Program;