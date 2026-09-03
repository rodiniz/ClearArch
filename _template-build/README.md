# SampleApp-style Layered Solution Template

This folder contains a reusable multi-project .NET template based on the same high-level structure as the current solution:

- `SampleApp.Api`
- `SampleApp.Application`
- `SampleApp.Domain`
- `SampleApp.Infrastructure`
- `SampleApp.Migrations`
- `SampleApp.Unit`

When the template is created with a name such as `Financer`, every project is renamed automatically:

- `Financer.Api`
- `Financer.Application`
- `Financer.Domain`
- `Financer.Infrastructure`
- `Financer.Migrations`
- `Financer.Unit`

## Install locally

```powershell
dotnet new install .\SampleApp.LayeredSolution
```

## Build as a NuGet template pack

```powershell
dotnet pack .\SampleApp.TemplatePack.csproj -c Release
```

This produces a `.nupkg` file that can be uploaded to an internal NuGet feed.

## Install from a package or feed

```powershell
dotnet new install SampleApp.TemplatePack::1.0.0 --nuget-source <FEED_URL>
```

## Create a solution

```powershell
dotnet new SampleApp-layered -n Financer
```

## Open in Visual Studio

After installation, the template is also available from the Visual Studio new project dialog as a .NET template.

## Notes

- The generated solution targets .NET 10.
- The generated solution uses SQLite so it runs without extra infrastructure.
- The `Migrations` project is included for future Entity Framework migrations.
- The sample feature is intentionally small and generic so it can be replaced with your domain-specific code.

## Template Evaluation

### Pros

- Clear project boundaries: the template starts with separate Api, Application, Domain, Infrastructure, Migrations, and Unit projects, which makes dependency direction and solution growth easier to manage.
- Fast local startup: SQLite plus the default connection string means a generated solution can run without provisioning SQL Server, containers, or cloud resources.
- Good packaging ergonomics: the template is already wired for `dotnet new` installation, local packing, and internal NuGet distribution.
- Built-in testing surface: the template includes a dedicated unit test project with AutoFixture, NSubstitute, xUnit, and FluentAssertions, which is a solid default stack for application-layer testing.
- Modern API defaults: OpenAPI, Scalar, health checks, and Wolverine HTTP endpoints provide a workable starting point for service development without extra setup.
- Messaging-ready structure: Wolverine configuration, handlers, middleware, events, and sagas are already present, which lowers the setup cost for teams that know they want asynchronous workflows.

### Cons

- More opinionated than a minimal clean architecture template: Wolverine is a strong architectural choice, but it adds framework concepts that many teams will need to learn or remove.
- The application abstraction still leaks EF Core concepts: exposing `DbSet<WorkItem>` through `IApplicationDbContext` keeps things practical, but it couples the application layer to Entity Framework instead of a persistence-agnostic model.
- The sample domain is intentionally thin: that keeps the template easy to rename, but it also means new teams still need to supply patterns for richer aggregates, validation, authorization, and cross-cutting concerns.
- The testing story is only partially mature out of the box: the test project exists and the basic conventions are good, but some sample tests are placeholders and not yet a strong example suite.
- The template is best for teams that already want layered architecture: for smaller CRUD services or prototypes, six projects plus messaging configuration may feel heavy relative to the problem size.

### Recommendation

This template is a strong starting point for teams that want a layered .NET service with clear boundaries, local-first setup, and room for messaging patterns. It is less suitable as a neutral baseline template because it makes architectural decisions early, especially around Wolverine, Entity Framework, and multi-project structure.