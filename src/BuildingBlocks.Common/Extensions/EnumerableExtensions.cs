using System.Diagnostics.CodeAnalysis;

namespace BuildingBlocks.Common.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? source) 
        => source is null || !source.Any();
}