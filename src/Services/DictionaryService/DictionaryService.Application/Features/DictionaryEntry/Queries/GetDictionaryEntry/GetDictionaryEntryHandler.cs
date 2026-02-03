using BuildingBlocks.Application.Abstractions;
using DictionaryService.Application.Abstractions;

namespace DictionaryService.Application.Features.DictionaryEntry.Queries.GetDictionaryEntry;

public sealed class GetDictionaryEntryHandler(IDictionaryEntryQueryService queryService)
    : IQueryHandler<GetDictionaryEntryQuery, GetDictionaryEntryResult>
{
    public async Task<GetDictionaryEntryResult> Handle(GetDictionaryEntryQuery request, CancellationToken cancellationToken)
    {
        var item = await queryService.GetByIdAsync(request.Id, cancellationToken);

        return new GetDictionaryEntryResult(item);
    }
}