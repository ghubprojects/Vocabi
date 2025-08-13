using Vocabi.Application.Common.Models;

namespace Vocabi.Application.Contracts.External.Dictionary;

public interface IMainDictionaryProvider : IExternalProvider
{
    Task<Result<List<DictionaryEntryModel>>> LookupAsync(string word);
}