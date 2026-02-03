using BuildingBlocks.Application.Abstractions;
using DictionaryService.Domain.Aggregates.DictionaryEntry;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace DictionaryService.Application.Features.DictionaryEntry.Commands.LookupDictionary;

public sealed class LookupDictionaryHandler(
    IDictionaryEntryRepository repository,
    ILogger<LookupDictionaryHandler> logger)
    : ICommandHandler<LookupDictionaryCommand, Result>
{
    public async Task<Result> Handle(LookupDictionaryCommand request, CancellationToken cancellationToken)
    {
        var keyword = request.Keyword.Trim().ToLowerInvariant();

        Result.
    }
}
