using Vocabi.Application.Common.Models;

namespace Vocabi.Application.Contracts.External.Dictionary;

public interface IDictionaryProvider : IExternalProvider
{
    Task<Result<DictionaryLookupResult>> LookupAsync(string word);
}