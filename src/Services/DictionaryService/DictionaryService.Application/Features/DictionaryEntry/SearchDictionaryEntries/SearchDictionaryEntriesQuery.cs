using MediatR;

namespace DictionaryService.Application.Features.DictionaryEntry.SearchDictionaryEntries;

public sealed record SearchDictionaryEntriesQuery(
    string Keyword
) : IRequest<SearchDictionaryEntriesResult>;