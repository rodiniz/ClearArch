using AutoFixture;
using CleanArch.Application.Common.Interfaces;
using CleanArch.Application.Features.WorkItems;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Interfaces.Repositories.Common;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CleanArch.Unit.Features.WorkItems;

public class CreateWorkItemCommandHandlerTests : BaseTest
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IDbSet<WorkItem> _workItems;
    private readonly CreateWorkItemCommandHandler _sut;

    public CreateWorkItemCommandHandlerTests()
    {
        _context = Fixture.FreezeSubstitute<IApplicationDbContext>();
        _dateTimeProvider = Fixture.FreezeSubstitute<IDateTimeProvider>();
        _workItems = Substitute.For<IDbSet<WorkItem>>();

        _context.Set<WorkItem>().Returns(_workItems);
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
            .AddAsync(Arg.Any<WorkItem>(), cancellationToken)
            .Returns(call =>
            {
                var item = call.Arg<WorkItem>();
                addedItem = item;
                return Task.FromResult(item);
            });

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

        await _workItems.Received(1).AddAsync(Arg.Any<WorkItem>(), cancellationToken);
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

        await _workItems.DidNotReceive().AddAsync(Arg.Any<WorkItem>(), Arg.Any<CancellationToken>());
        await _context.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
    }
}