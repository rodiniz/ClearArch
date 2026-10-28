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

## Create a solution

The template targets .NET 10 and includes API, Application, Domain,
Infrastructure, Migrations, and Unit test projects.

```powershell
dotnet new CleanArch-layered -n Example
```

### Optional messaging transports

By default, Wolverine runs locally without external broker packages,
connections, or credentials. Azure Service Bus and AWS SQS are opt-in:

| Option | Default | Integration |
| --- | --- | --- |
| `--UseAzureServiceBus` | `false` | Azure Service Bus |
| `--UseAwsSqs` | `false` | AWS SQS |

```powershell
dotnet new CleanArch-layered -n Example --UseAzureServiceBus
dotnet new CleanArch-layered -n Example --UseAwsSqs
```

Selected transports include their packages, configuration, and Wolverine
registration. Configure them through application settings or environment variables:

- **Azure Service Bus:** Set `AzureSettings__ServiceBusNameSpace` to the fully
	qualified namespace and `AzureSettings__ServiceBusQueueName` to an existing queue.
	Authentication uses `DefaultAzureCredential` by default, supporting managed
	identity. To use a connection string, set
	`AzureSettings__UseManagedIdentity=false` and supply
	`AzureSettings__ServiceBusConnectionString` through environment variables or user secrets.
- **AWS SQS:** Set `AwsSettings__SqsQueueName` to an existing queue and
	`AwsSettings__Region` to its region, or use standard AWS SDK region configuration.
	Authentication uses the AWS SDK credential chain, including IAM roles and shared profiles.

Keep credentials out of source control. Provision queues before running the
application and add message-specific publish routes for outgoing messages.
See [the template-pack guide](README.TemplatePack.md) for additional configuration
details and transport references.

## Publish and install from a feed

To publish it to a feed:

```powershell
dotnet nuget push .\nupkg\CleanArch.TemplatePack.1.0.0.nupkg --source <FEED_NAME_OR_URL> --api-key <API_KEY>
```

To install from that feed:

```powershell
dotnet new install CleanArch.TemplatePack::1.0.0 --nuget-source <FEED_URL>
```