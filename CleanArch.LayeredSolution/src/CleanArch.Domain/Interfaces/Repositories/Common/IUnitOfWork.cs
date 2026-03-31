namespace CleanArch.Domain.Interfaces.Repositories.Common;

using CleanArch.Domain.Interfaces.Repositories;
using FluentResults;


public interface IUnitOfWork : IDisposable
{
	IRepository<T> Repository<T>() where T : class, new();

	Task<Result> CompleteAsync();
}   