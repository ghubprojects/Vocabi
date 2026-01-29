using MediatR;

namespace DictionaryService.Application.Features.DictionaryEntry.GetDictionaryEntry;

public sealed record GetDictionaryEntryQuery(
    Guid Id
) : IRequest<GetDictionaryEntryResult>;