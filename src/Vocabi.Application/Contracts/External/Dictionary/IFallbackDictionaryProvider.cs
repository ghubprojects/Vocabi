using FluentResults;

namespace Vocabi.Application.Contracts.External.Dictionary;

public interface IFallbackDictionaryProvider : IExternalProvider
{
    Task<Result<List<string>>> LookupAsync(string word);
}