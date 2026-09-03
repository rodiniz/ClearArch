using AutoFixture;
using SampleApp.Application.Common.Interfaces;
using SampleApp.Application.Features.WorkItems;
using SampleApp.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace SampleApp.Unit.Features.WorkItems;

public class CreateWorkItemCommandHandlerTests : BaseTest
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly DbSet<WorkItem> _workItems;
    private readonly CreateWorkItemCommandHandler _sut;

    public CreateWorkItemCommandHandlerTests()
    {
        _context = Fixture.FreezeSubstitute<IApplicationDbContext>();
        _dateTimeProvider = Fixture.FreezeSubstitute<IDateTimeProvider>();
        _workItems = Substitute.For<DbSet<WorkItem>>();

        _context.WorkItems.Returns(_workItems);
        _sut = Fixture.Create<CreateWorkItemCommandHandler>();
    }

    [Fact]
    public async Task GivenValidTitle_WhenHandlingCommand_ThenReturnsCreatedWorkItem()
    {
        // Arrange
        var createdAtUtc = Fixture.Create<DateTime>();
        var cancellationToken = Fixture.Create<CancellationToken>();
        var command = new CreateWorkItemCommand("  Ship release checklist  ");
        WorkItem? addedItem = null;

        _dateTimeProvider.UtcNow.Returns(createdAtUtc);
        _workItems
            .When(set => set.Add(Arg.Any<WorkItem>()))
            .Do(call => addedItem = call.Arg<WorkItem>());

        // Act
        var result = await _sut.Handle(command, cancellationToken);

        // Assert
        addedItem.Should().NotBeNull();
        addedItem!.Title.Should().Be("Ship release checklist");
        addedItem.IsDone.Should().BeFalse();
        addedItem.CreatedAtUtc.Should().Be(createdAtUtc);

        result.Id.Should().Be(addedItem.Id);
        result.Title.Should().Be("Ship release checklist");
        result.IsDone.Should().BeFalse();
        result.CreatedAtUtc.Should().Be(createdAtUtc);

        _workItems.Received(1).Add(Arg.Any<WorkItem>());
        await _context.Received(1).SaveChangesAsync(cancellationToken);
    }

    [Fact]
    public async Task GivenWhitespaceTitle_WhenHandlingCommand_ThenThrowsValidationException()
    {
        // Arrange
        var command = new CreateWorkItemCommand("   ");

        // Act
        var act = () => _sut.Handle(command, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<ArgumentException>();
        exception.Which.ParamName.Should().Be("Title");
        exception.Which.Message.Should().StartWith("Title is required.");

        _workItems.DidNotReceive().Add(Arg.Any<WorkItem>());
        await _context.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
    }
}