using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Dictionary;
using Vocabi.Shared.Extensions;

namespace Vocabi.Application.Services;

public class DictionaryLookupService(IMainDictionaryProvider mainDictionaryProvider, IFallbackDictionaryProvider fallbackDictionaryProvider)
{
    public async Task<Result<List<DictionaryEntryModel>>> LookupAsync(string word)
    {
        var lookupResult = await mainDictionaryProvider.LookupAsync(word);
        if (lookupResult.IsFailure)
            return Result<List<DictionaryEntryModel>>.Failure([.. lookupResult.Errors]);

        Result<List<string>>? fallbackLookupResult = null;
        if (lookupResult.Data.Select(x => x.Meanings).IsNullOrEmpty())
        {
            foreach (var entry in lookupResult.Data)
            {
                fallbackLookupResult ??= await fallbackDictionaryProvider.LookupAsync(word);
                if (fallbackLookupResult.IsFailure)
                    return Result<List<DictionaryEntryModel>>.Failure([.. fallbackLookupResult.Errors]);

                entry.Meanings.AddRange(fallbackLookupResult.Data);
            }
        }

        return Result<List<DictionaryEntryModel>>.Success(lookupResult.Data);
    }
}