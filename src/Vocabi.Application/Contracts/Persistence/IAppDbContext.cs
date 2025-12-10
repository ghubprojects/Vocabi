using Microsoft.EntityFrameworkCore.Storage;

namespace Vocabi.Application.Contracts.Persistence;

public interface IAppDbContext
{
    bool HasActiveTransaction { get; }

    IExecutionStrategy CreateExecutionStrategy();

    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync(IDbContextTransaction transaction);
}