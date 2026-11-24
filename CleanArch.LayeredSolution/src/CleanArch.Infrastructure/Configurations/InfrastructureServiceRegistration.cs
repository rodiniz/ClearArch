using CleanArch.Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;


namespace CleanArch.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public static class InfrastructureServiceRegistration
{
	private static readonly Assembly Assembly = typeof(InfrastructureServiceRegistration).Assembly;

	public static IServiceCollection AddInfrastructureComponents(this IServiceCollection services)
	{
		return services			
			.RegisterImplementedTypes(Assembly, ServiceLifetime.Scoped,
				typeof(IRepository<>));
	}
}
