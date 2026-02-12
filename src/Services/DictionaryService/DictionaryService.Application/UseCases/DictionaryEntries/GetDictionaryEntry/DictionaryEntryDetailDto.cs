namespace DictionaryService.Application.UseCases.DictionaryEntries.GetDictionaryEntry;

public sealed record DictionaryEntryDetailDto(
   Guid Id,
   string Headword,
   string PartOfSpeech,
   string Pronunciation,
   string Source,
   IReadOnlyList<string> Definitions,
   IReadOnlyList<string> Examples
);