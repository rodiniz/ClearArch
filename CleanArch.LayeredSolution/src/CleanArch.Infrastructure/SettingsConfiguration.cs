using Azure.Identity;
using CleanArch.Infrastructure.Extensions;
using CleanArch.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.Infrastructure;

public static class SettingsConfiguration
{
	public static IServiceCollection ConfigureSettings(this IServiceCollection services,
		ConfigurationManager configurationManager)
	{
		var npgsqlSettings = configurationManager.GetSettings<NpgsqlSettings>();

		return services
		   .AddSingleton(npgsqlSettings);
	}
}
