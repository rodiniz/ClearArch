namespace CleanArch.Domain.Interfaces.Repositories;

using FluentResults;

using System.Linq.Expressions;

public interface IRepository<T> where T : class, new()
{
	Task<Result> CreateAsync(T entity);

	Task<Result<T>> GetByIdAsync(Guid uniqueIdentifier);

	Task<Result> DeleteAsync(Expression<Func<T, bool>> predicate);

	Task<Result<T?>> FindAsync(Expression<Func<T, bool>> predicate);

	Task<Result<List<T>>> FindManyAsync(Expression<Func<T, bool>> predicate);
}