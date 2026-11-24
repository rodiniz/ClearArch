using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace CleanArch.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public static class TypesRegistration
{
	public static IServiceCollection RegisterImplementedTypes(
		this IServiceCollection services,
		Assembly assemblies,
		ServiceLifetime lifetime,
		params Type[] types)
	{
		return services.Scan(config =>
			config.FromAssemblies(assemblies)
				.AddClasses(filter => filter.AssignableToAny(types))
				.AsImplementedInterfaces()
				.WithLifetime(lifetime));
	}
}
