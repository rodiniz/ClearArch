using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;
using CleanArch.Domain.Interfaces.Repositories.Common;

[ExcludeFromCodeCoverage]
public class DbSetWrapper<T> : DbSet<T>, IDbSet<T>
	where T : class
{
	private readonly DbSet<T> dbSet;
	private readonly DbContext dbContext;

	public DbSetWrapper(DbSet<T> dbSet, DbContext dbContext)
	{
		this.dbSet = dbSet;
		this.dbContext = dbContext;
	}

	public new Task Attach(T entity)
	{
		var attachedEntity = dbSet.Attach(entity);
		attachedEntity.State = EntityState.Unchanged;

		return Task.CompletedTask;
	}

	public new async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
	{
		var addedEntityEntry = await dbSet.AddAsync(entity, cancellationToken);
		return addedEntityEntry.Entity;
	}

	public new async Task<T?> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
	{
		return await dbSet.FindAsync(keyValues, cancellationToken);
	}

	public new ValueTask<T?> FindAsync(params object[] keyValues)
	{
		return dbSet.FindAsync(keyValues);
	}

	public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await dbSet.AnyAsync(predicate, cancellationToken);
	}

	public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
	{
		return await dbSet.AnyAsync(cancellationToken);
	}

	public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await dbSet.CountAsync(predicate, cancellationToken);
	}

	public async Task<int> CountAsync(CancellationToken cancellationToken = default)
	{
		return await dbSet.CountAsync(cancellationToken);
	}

	public async Task<List<T>> ToListAsync(CancellationToken cancellationToken = default)
	{
		return await dbSet.ToListAsync(cancellationToken);
	}

	public async Task<T[]> ToArrayAsync(CancellationToken cancellationToken = default)
	{
		return await dbSet.ToArrayAsync(cancellationToken);
	}

	public async Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await dbSet.FirstAsync(predicate, cancellationToken);
	}

	public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
	}

	public async Task<T> SingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await dbSet.SingleAsync(predicate, cancellationToken);
	}

	public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await dbSet.SingleOrDefaultAsync(predicate, cancellationToken);
	}

	public async Task<T?> UpdateAsync(T entity, CancellationToken cancellationToken = default)
	{
		var existingEntity = await dbSet.FindAsync(GetKeyValues(entity), cancellationToken);

		if (existingEntity != null)
		{
			// Update the existing entity with the new values
			dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
		}

		return existingEntity;
	}

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return await dbContext.SaveChangesAsync(cancellationToken);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return dbSet.AsQueryable().GetEnumerator();
	}

	IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken)
	{
		return ((IAsyncEnumerable<T>)dbSet).GetAsyncEnumerator(cancellationToken);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return dbSet.AsQueryable().GetEnumerator();
	}

	public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
	{
		return dbSet.Where(predicate);
	}

	public async Task<List<T>> WhereToListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await Where(predicate).ToListAsync(cancellationToken);
	}

	public override IEntityType EntityType => dbSet.EntityType;

	public Type ElementType => typeof(T);

	public Expression Expression => dbSet.AsQueryable().Expression;

	public IQueryProvider Provider => dbSet.AsQueryable().Provider;

	private object?[] GetKeyValues(T entity)
	{
		var entityType = dbContext.Model.FindEntityType(typeof(T))
			?? throw new InvalidOperationException($"Entity of type {typeof(T).Name} is not part of the model.");
		var key = entityType.FindPrimaryKey();

		if (key != null)
		{
			return key.Properties.Select(p => p.GetGetter().GetClrValue(entity)).ToArray();
		}

		throw new InvalidOperationException($"Entity of type {typeof(T).Name} does not have a primary key defined.");
	}
}