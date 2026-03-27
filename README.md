# Template Packaging

The reusable Visual Studio/.NET template is stored in `CleanArch.LayeredSolution`.

To produce a distributable NuGet template pack:

```powershell
dotnet pack .\CleanArch.TemplatePack.csproj -c Release
```

The generated package is written to `nupkg`.

## Install without publishing to NuGet

Install directly from the template folder:

```powershell
dotnet new install .\CleanArch.LayeredSolution
```

Or install from a local package file after packing:

```powershell
dotnet new install .\nupkg\CleanArch.TemplatePack.1.0.0.nupkg
```

To publish it to a feed:

```powershell
dotnet nuget push .\nupkg\CleanArch.TemplatePack.1.0.0.nupkg --source <FEED_NAME_OR_URL> --api-key <API_KEY>
```

To install from that feed:

```powershell
dotnet new install CleanArch.TemplatePack::1.0.0 --nuget-source <FEED_URL>
```