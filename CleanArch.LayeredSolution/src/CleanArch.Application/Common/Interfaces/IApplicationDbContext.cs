using Microsoft.EntityFrameworkCore;
using CleanArch.Domain.Entities;

namespace CleanArch.Application.Common.Interfaces;

public interface IApplicationDbContext: IDisposable
{  	
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}   