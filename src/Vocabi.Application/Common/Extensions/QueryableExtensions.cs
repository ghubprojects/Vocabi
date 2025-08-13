using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Vocabi.Application.Common.Models;

namespace Vocabi.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedData<TResult>> ProjectToPaginatedDataAsync<TSource, TResult>(
        this IQueryable<TSource> source,
        int pageIndex,
        int pageSize,
        IConfigurationProvider configuration) 
        where TResult : class
    {
        var count = await source.CountAsync();

        var items = await source
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ProjectTo<TResult>(configuration)
            .ToListAsync();

        return new PaginatedData<TResult>(items, count, pageIndex, pageSize);
    }
}