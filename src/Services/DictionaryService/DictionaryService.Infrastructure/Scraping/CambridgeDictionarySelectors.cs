namespace DictionaryService.Infrastructure.Scraping;

/// <summary>
/// Centralized CSS selectors for scraping Cambridge Dictionary entries,
/// ordered by priority with fallbacks to tolerate layout and HTML variations.
/// </summary>
internal static class CambridgeDictionarySelectors
{
    public static readonly string[] EntryBlock =
    [
        ".dictionary[data-id='cald4'] .entry-body__el",
        ".dictionary[data-id='cacd'] .entry-body__el",
        ".dictionary[data-id='cald4'] .idiom-block",
        ".dictionary[data-id='cacd'] .idiom-block",
    ];

    public static readonly string[] Headword =
    [
        ".dhw",
        ".hw",
    ];

    public static readonly string[] PartOfSpeech =
    [
        ".dpos",
        ".pos",
    ];

    public static readonly string[] Pronunciation =
    [
        ".us .dpron .dipa",
        ".us .pron .ipa",
    ];

    public static readonly string[] DefinitionBlock =
    [
        ".dsense_b > .ddef_block",
        ".sense-body > .def-block"
    ];

    public static readonly string[] DefinitionText =
    [
        ".def",
        ".ddef_h"
    ];

    public static readonly string[] Examples =
    [
        ".examp",
        ".eg"
    ];
}
