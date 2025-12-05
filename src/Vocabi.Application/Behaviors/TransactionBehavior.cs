using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Vocabi.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly DbContext _db;
    public TransactionBehavior(DbContext db) => _db = db;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var isCommand = typeof(ICommand).IsAssignableFrom(typeof(TRequest));
        if (!isCommand) return await next();

        await using var txn = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            var response = await next();
            await _db.SaveChangesAsync(ct);
            await txn.CommitAsync(ct);
            return response;
        }
        catch
        {
            await txn.RollbackAsync(ct);
            throw;
        }
    }
}
