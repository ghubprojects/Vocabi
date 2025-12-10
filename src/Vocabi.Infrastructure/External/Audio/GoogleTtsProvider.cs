using FluentResults;
using Microsoft.AspNetCore.WebUtilities;
using Vocabi.Application.Contracts.External.Audio;

namespace Vocabi.Infrastructure.External.Audio;

public class GoogleTtsProvider : IAudioProvider
{
    public string ProviderName => "Google TTS";

    public const string BaseUrl = "https://translate.google.com/translate_tts";

    public Result<string> Get(string text, string lang = "en")
        => Result.Ok(
            QueryHelpers.AddQueryString(BaseUrl, new Dictionary<string, string?>
            {
                ["ie"] = "UTF-8",
                ["tl"] = lang,
                ["client"] = "tw-ob",
                ["q"] = text
            }));
}
