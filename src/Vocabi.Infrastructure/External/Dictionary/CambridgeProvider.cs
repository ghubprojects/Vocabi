using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Dictionary;

namespace Vocabi.Infrastructure.External.Cambridge;

public class CambridgeProvider : IDictionaryProvider
{
    public string ProviderName => "Cambridge Dictionary";

    public Task<Result<DictionaryLookupResult>> LookupAsync(string word)
    {
        throw new NotImplementedException();
    }
}
