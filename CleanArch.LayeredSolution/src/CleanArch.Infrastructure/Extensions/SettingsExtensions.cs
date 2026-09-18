
namespace CleanArch.Infrastructure.Extensions;

using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public static class SettingsExtensions
{
	public static T GetSettings<T>(this ConfigurationManager configurationManager)
	{
		return configurationManager
			.GetSection(typeof(T).Name)
			.Get<T>();
	}
}
