# CleanArch Template Pack

This NuGet package contains the `CleanArch-layered` .NET solution template.

## Install without publishing to NuGet

Install directly from the template source folder:

```powershell
dotnet new install .\CleanArch.LayeredSolution
```

Or install from a local `.nupkg` file:

```powershell
dotnet new install .\nupkg\CleanArch.TemplatePack.1.0.0.nupkg
```

## Install from a NuGet package

```powershell
dotnet new install CleanArch.TemplatePack::1.0.0 --nuget-source <FEED_URL>
```

## Create a solution

```powershell
dotnet new CleanArch-layered -n Example
```

The generated solution includes:

- `Example.Api`
- `Example.Application`
- `Example.Domain`
- `Example.Infrastructure`
- `Example.Migrations`
- `Example.Unit`

## Publish the package

```powershell
dotnet nuget push .\nupkg\CleanArch.TemplatePack.1.0.0.nupkg --source <FEED_NAME_OR_URL> --api-key <API_KEY>
```