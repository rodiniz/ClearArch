using CleanArch.Application.Common.Interfaces;

namespace CleanArch.Infrastructure.Services;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}