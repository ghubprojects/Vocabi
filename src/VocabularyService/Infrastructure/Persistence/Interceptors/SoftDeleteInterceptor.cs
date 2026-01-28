using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace VocabularyService.Infrastructure.Persistence.Interceptors;

public sealed class SoftDeleteInterceptor(ICurrentUser currentUser, TimeProvider timeProvider) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var userId = currentUser.Session?.UserId;
        var now = timeProvider.GetUtcNow();

        foreach (var entry in context.ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State is EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.SoftDelete.IsDeleted = true;
                entry.Entity.SoftDelete.DeletedAt = now;
                entry.Entity.SoftDelete.DeletedBy = userId;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}