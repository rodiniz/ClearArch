using CleanArch.Application.Common.Interfaces;
using CleanArch.Domain.Interfaces.Repositories;
using FluentResults;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infrastructure.Persistence;

public class Repository<T> : IRepository<T> where T : class, new()
{
	protected readonly IApplicationDbContext context;
	protected readonly ILog logger;

	public Repository(ILog logger, IApplicationDbContext context)
	{
		this.context = context;
		this.logger = logger;
	}

	public async Task<Result> CreateAsync(T entity)
	{
		try
		{
			await context.Set<T>().AddAsync(entity);

			return Result.Ok();
		}
		catch (Exception ex)
		{
			string errorMsg = $"Failed to insert {nameof(T)}.";

			logger.Error(errorMsg, ex, ("Entity", entity));

			return Result.Fail(new Error(errorMsg).CausedBy(ex));
		}
	}

	public async Task<Result<T?>> GetByIdAsync(Guid uniqueIdentifier)
	{
		try
		{
			var result = await context.Set<T>().FindAsync(uniqueIdentifier);

			return Result.Ok(result);
		}
		catch (Exception ex)
		{
			string errorMsg = $"Failed to get by id {nameof(T)}.";

			logger.Error(errorMsg, ex, ("Id", uniqueIdentifier));

			return Result.Fail(new Error(errorMsg).CausedBy(ex));
		}
	}
	public async Task<Result> DeleteAsync(Expression<Func<T, bool>> predicate)
	{
		try
		{
			await context.Set<T>()
				.Where(predicate)
				.ExecuteDeleteAsync();
			return Result.Ok();
		}
		catch (Exception ex)
		{
			string errorMsg = $"Failed to delete {nameof(T)}.";

			logger.Error(errorMsg, ex, ("Predicate", predicate.ToString()));

			return Result.Fail(new Error(errorMsg).CausedBy(ex));
		}
	}

	public async Task<Result<T?>> FindAsync(Expression<Func<T, bool>> predicate)
	{
		try
		{
			var result = await context.Set<T>()
				.FirstOrDefaultAsync(predicate);

			return Result.Ok(result);
		}
		catch (Exception ex)
		{
			string errorMsg = $"Failed to find {nameof(T)}.";

			logger.Error(errorMsg, ex, ("Predicate", predicate.ToString()));

			return Result.Fail(new Error(errorMsg).CausedBy(ex));
		}
	}

	public async Task<Result<List<T>>> FindManyAsync(Expression<Func<T, bool>> predicate)
	{
		try
		{
			var result = await context.Set<T>()
				.Where(predicate)
				.ToListAsync();

			return Result.Ok(result);
		}
		catch (Exception ex)
		{
			string errorMsg = $"Failed to find many {nameof(T)}.";

			logger.Error(errorMsg, ex, ("Predicate", predicate.ToString()));

			return Result.Fail(new Error(errorMsg).CausedBy(ex));
		}
	}
}
