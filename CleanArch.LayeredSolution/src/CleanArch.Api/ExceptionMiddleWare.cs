using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.Api;
public static class ExceptionMiddleWare
{
    public static void UseExceptionMiddleWare(this IApplicationBuilder app)
    {
       app.Use(async (context, next) => {
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
    }
}