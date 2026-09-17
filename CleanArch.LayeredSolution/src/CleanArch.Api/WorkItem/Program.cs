using CleanArch.Application;
using CleanArch.Infrastructure;
using Scalar.AspNetCore;
using Wolverine;
using Wolverine.Http;
using CleanArch.Api;
using JasperFx;

var builder = WebApplication.CreateBuilder(args);

builder.AddWolverine();

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

return await app.RunJasperFxCommands(args);

public partial class Program;