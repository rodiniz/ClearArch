namespace CleanArch.Domain.Entities;

public class WorkItem
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public bool IsDone { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}