using AngleSharp.Dom;
using System.Text.RegularExpressions;

namespace DictionaryService.Infrastructure.Scraping;

internal static class AngleSharpDomExtensions
{
    public static IElement? SelectElement(this IParentNode node, IEnumerable<string> selectors)
    {
        foreach (var selector in selectors)
        {
            var element = node.QuerySelector(selector);
            if (element is not null)
                return element;
        }

        return null;
    }

    public static IReadOnlyList<IElement> SelectElements(this IParentNode node, IEnumerable<string> selectors)
    {
        foreach (var selector in selectors)
        {
            var elements = node.QuerySelectorAll(selector);
            if (elements.Length > 0)
                return elements;
        }

        return [];
    }

    public static string SelectText(this IParentNode node, IEnumerable<string> selectors)
    {
        var content = node.SelectElement(selectors)?.TextContent;
        return Normalize(content);
    }

    public static IReadOnlyList<string> SelectTexts(this IParentNode node, IEnumerable<string> selectors)
    {
        return node
            .SelectElements(selectors)
            .Select(e => e.TextContent.Trim())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToArray();
    }

    private static string Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : Regex.Replace(value, @"\s+", " ").Trim();
}
