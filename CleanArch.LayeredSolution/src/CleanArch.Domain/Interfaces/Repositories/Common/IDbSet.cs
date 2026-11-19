namespace CleanArch.Domain.Interfaces.Repositories.Common;

public interface IDbSet<T> : IQueryable<T>, IAsyncEnumerable<T>
	where T : class
{
	Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
	ValueTask<T?> FindAsync(params object[] keyValues);
}