using CleanArch.Domain.Interfaces.Repositories.Common;

namespace CleanArch.Application.Common.Interfaces;

public interface IApplicationDbContext: IDisposable
{
	IDbSet<T> Set<T>() where T : class;

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}