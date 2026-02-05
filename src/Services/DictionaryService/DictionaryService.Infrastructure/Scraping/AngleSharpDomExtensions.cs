using AngleSharp.Dom;

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

    public static string? SelectText(this IParentNode node, IEnumerable<string> selectors)
    {
        return node
            .SelectElement(selectors)
            ?.TextContent
            .Trim();
    }

    public static IReadOnlyList<string> SelectTexts(this IParentNode node, IEnumerable<string> selectors)
    {
        return node
            .SelectElements(selectors)
            .Select(e => e.TextContent.Trim())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToArray();
    }
}
