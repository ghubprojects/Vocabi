using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<TResult>> ProjectToPagedResultAsync<TSource, TResult>(
        this IQueryable<TSource> source,
        int pageIndex,
        int pageSize,
        IConfigurationProvider configuration,
        CancellationToken cancellationToken)
        where TResult : class
    {
        var count = await source.CountAsync(cancellationToken);

        var items = await source
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ProjectTo<TResult>(configuration)
            .ToListAsync(cancellationToken);

        return new PagedResult<TResult>(items, count, pageIndex, pageSize);
    }
}