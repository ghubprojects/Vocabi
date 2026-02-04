namespace DictionaryService.Application.UseCases.DictionaryEntries.Dtos;

public sealed record DictionaryEntryDetail(
   Guid Id,
   string Headword,
   string PartOfSpeech,
   string Pronunciation,
   string Source,
   IReadOnlyList<string> Definitions,
   IReadOnlyList<string> Examples
);