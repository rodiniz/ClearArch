# CleanArch Template Pack

This NuGet package contains the `CleanArch-layered` .NET solution template.

## Install from NuGet.org

```powershell
dotnet new install CleanArch.TemplatePack
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

## Optional PostgreSQL provider

PostgreSQL support is excluded by default. Generate a solution with the Npgsql
driver and EF Core provider by opting in:

```powershell
dotnet new CleanArch-layered -n Example --UsePostgres
```

Set `ConnectionStrings__DefaultConnection` to the PostgreSQL connection string
before using the work item endpoints. Without `--UsePostgres`, no database provider
is selected; configure another EF Core provider before using persistence-backed
endpoints.

## Publish the package

```powershell
dotnet nuget push .\nupkg\CleanArch.TemplatePack.1.0.0.nupkg --source <FEED_NAME_OR_URL> --api-key <API_KEY>
```