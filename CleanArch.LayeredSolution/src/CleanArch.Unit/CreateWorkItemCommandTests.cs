using AutoFixture;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using CleanArch.Application.Common.Interfaces;
using CleanArch.Application.Features.WorkItems;
using CleanArch.Infrastructure.Persistence;
using Xunit;

namespace CleanArch.Unit;

public class CreateWorkItemCommandTests : BaseTest
{
    [Fact]
    public async Task Handle_ShouldCreateWorkItem()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();

        Fixture.Register<IApplicationDbContext>(() => context);
        Fixture.Register<IDateTimeProvider>(() => new FakeDateTimeProvider());

        var expectedTitle = Fixture.Create<string>().Trim();
        var command = Fixture.Build<CreateWorkItemCommand>()
            .With(item => item.Title, expectedTitle)
            .Create();

        var handler = Fixture.Create<CreateWorkItemCommandHandler>();

        var result = await handler.Handle(command, CancellationToken.None);

        result.Title.Should().Be(expectedTitle);
        result.IsDone.Should().BeFalse();
        result.CreatedAtUtc.Should().Be(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        context.WorkItems.Should().ContainSingle();
    }

    private sealed class FakeDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}