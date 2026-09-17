namespace CleanArch.Api;

using JasperFx.CodeGeneration;
using JasperFx.CodeGeneration.Model;
using Wolverine;
using Wolverine.FluentValidation;

public static class WolverineExtensions
{
	public static void AddWolverine(this WebApplicationBuilder builder)
    {
        builder.Host.UseWolverine(options =>
        {
            // Auto-discovery of message handlers and events
            options.Discovery.IncludeAssembly(typeof(Application.DependencyInjection).Assembly);

			options.ServiceLocationPolicy = ServiceLocationPolicy.AlwaysAllowed;
			if (builder.Environment.IsDevelopment())
			{
				options.CodeGeneration.TypeLoadMode = TypeLoadMode.Auto;
			}
			else
			{
				options.CodeGeneration.TypeLoadMode = TypeLoadMode.Static;
			}

			options.UseFluentValidation();
           
        });
    }
}