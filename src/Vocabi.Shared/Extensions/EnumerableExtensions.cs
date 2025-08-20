using System.Diagnostics.CodeAnalysis;

namespace Vocabi.Shared.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? source)
    {
        return source is null || !source.Any();
    }
}