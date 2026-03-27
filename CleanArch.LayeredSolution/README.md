# CleanArch-style Layered Solution Template

This folder contains a reusable multi-project .NET template based on the same high-level structure as the current solution:

- `CleanArch.Api`
- `CleanArch.Application`
- `CleanArch.Domain`
- `CleanArch.Infrastructure`
- `CleanArch.Migrations`
- `CleanArch.Unit`

When the template is created with a name such as `Financer`, every project is renamed automatically:

- `Financer.Api`
- `Financer.Application`
- `Financer.Domain`
- `Financer.Infrastructure`
- `Financer.Migrations`
- `Financer.Unit`

## Install locally

```powershell
dotnet new install .\CleanArch.LayeredSolution
```

## Build as a NuGet template pack

```powershell
dotnet pack .\CleanArch.TemplatePack.csproj -c Release
```

This produces a `.nupkg` file that can be uploaded to an internal NuGet feed.

## Install from a package or feed

```powershell
dotnet new install CleanArch.TemplatePack::1.0.0 --nuget-source <FEED_URL>
```

## Create a solution

```powershell
dotnet new CleanArch-layered -n Financer
```

## Open in Visual Studio

After installation, the template is also available from the Visual Studio new project dialog as a .NET template.

## Notes

- The generated solution targets .NET 10.
- The generated solution uses SQLite so it runs without extra infrastructure.
- The `Migrations` project is included for future Entity Framework migrations.
- The sample feature is intentionally small and generic so it can be replaced with your domain-specific code.