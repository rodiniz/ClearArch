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

## Optional messaging transports

The default template uses Wolverine locally, with no external broker packages,
connections, or credentials required. Select transports explicitly:

```powershell
dotnet new CleanArch-layered -n Example --UseAzureServiceBus
dotnet new CleanArch-layered -n Example --UseAwsSqs
```

Both options default to `false` and can be combined. Selected transports are
configured by the API's existing Wolverine registration.

For Azure Service Bus, set `AzureSettings__ServiceBusNameSpace` to the fully
qualified namespace (for example, `example.servicebus.windows.net`). Managed
identity is enabled by default via `DefaultAzureCredential`. For connection-string
authentication, set `AzureSettings__UseManagedIdentity=false` and supply
`AzureSettings__ServiceBusConnectionString` through environment variables or user
secrets. Set `AzureSettings__ServiceBusQueueName` to an existing queue.

For AWS SQS, set `AwsSettings__Region` or use the AWS SDK's standard region
configuration, and set `AwsSettings__SqsQueueName` to an existing queue name.
Authentication uses the AWS SDK credential chain, including IAM roles and shared
AWS profiles. No access keys are stored in the template. Queues are not created
automatically. Add message-specific publish routes as needed.

Transport references: [Azure Service Bus](https://wolverinefx.net/guide/messaging/transports/azureservicebus/)
and [Amazon SQS](https://wolverinefx.net/guide/messaging/transports/sqs/).

## Publish the package

```powershell
dotnet nuget push .\nupkg\CleanArch.TemplatePack.1.0.0.nupkg --source <FEED_NAME_OR_URL> --api-key <API_KEY>
```