using SampleApp.Application.Common.Interfaces;

namespace SampleApp.Infrastructure.Services;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}