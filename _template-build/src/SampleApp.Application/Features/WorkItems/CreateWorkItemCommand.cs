using FluentValidation;
using SampleApp.Application.Common.Interfaces;
using SampleApp.Domain.Entities;

namespace SampleApp.Application.Features.WorkItems;

public sealed record CreateWorkItemCommand(string Title);

public sealed class CreateWorkItemCommandValidator : AbstractValidator<CreateWorkItemCommand>
{
    public CreateWorkItemCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}

public sealed class CreateWorkItemCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<WorkItemDto> Handle(CreateWorkItemCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException("Title is required.", nameof(request.Title));
        }

        var item = new WorkItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            CreatedAtUtc = dateTimeProvider.UtcNow,
            IsDone = false
        };

        context.WorkItems.Add(item);
        await context.SaveChangesAsync(cancellationToken);

        return new WorkItemDto(item.Id, item.Title, item.IsDone, item.CreatedAtUtc);
    }
}