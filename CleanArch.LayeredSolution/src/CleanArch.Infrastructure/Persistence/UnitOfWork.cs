using CleanArch.Application.Common.Interfaces;
using CleanArch.Domain.Interfaces.Repositories;
using CleanArch.Domain.Interfaces.Repositories.Common;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;

public sealed class UnitOfWork : IUnitOfWork
{
	private readonly ILog logger;
	private readonly IApplicationDbContext context;
	private readonly IServiceProvider serviceProvider;
	private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

	public UnitOfWork(ILog logger,
		IApplicationDbContext context,
		IServiceProvider serviceProvider)
	{
		this.logger = logger;
		this.context = context;
		this.serviceProvider = serviceProvider;
	}

	public async Task<Result> CompleteAsync()
	{
		try
		{
			await context.SaveChangesAsync();

			return Result.Ok();
		}
		catch (Exception ex)
		{
			const string errorMessage = "Exception ocorred saving data.";
			logger.Error(errorMessage, ex);

			return Result.Fail(errorMessage);
		}
	}

	public IRepository<T> Repository<T>() where T : class, new()
	{
		if (!repositories.ContainsKey(typeof(T)))
		{
			var repository = serviceProvider.GetService<IRepository<T>>();

			repositories.Add(typeof(T), repository);

			return repository;
		}

		return repositories[typeof(T)] as IRepository<T>;
	}

	public void Dispose()
	{
		context.Dispose();
	}
}