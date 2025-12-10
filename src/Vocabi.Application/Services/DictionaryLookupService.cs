using FluentResults;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Dictionary;
using Vocabi.Shared.Extensions;

namespace Vocabi.Application.Services;

public class DictionaryLookupService(IMainDictionaryProvider mainDictionaryProvider, IFallbackDictionaryProvider fallbackDictionaryProvider)
{
    public async Task<Result<List<DictionaryEntryModel>>> LookupAsync(string word)
    {
        var lookupResult = await mainDictionaryProvider.LookupAsync(word);
        if (lookupResult.IsFailed)
            return Result.Fail([.. lookupResult.Errors]);

        Result<List<string>>? fallbackLookupResult = null;
        if (lookupResult.Value.Select(x => x.Meanings).IsNullOrEmpty())
        {
            foreach (var entry in lookupResult.Value)
            {
                fallbackLookupResult ??= await fallbackDictionaryProvider.LookupAsync(word);
                if (fallbackLookupResult.IsFailed)
                    return Result.Fail([.. fallbackLookupResult.Errors]);

                entry.Meanings.AddRange(fallbackLookupResult.Value);
            }
        }

        return Result.Ok(lookupResult.Value);
    }
}