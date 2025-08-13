using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Audio;

namespace Vocabi.Infrastructure.External.Audio;

public class GoogleTtsProvider: IAudioProvider
{
    public string ProviderName => "Google TTS";

    public async Task<Result<string>> GetAsync(string text, string lang = "en")
    {
        return Result<string>.Success($"https://translate.google.com/translate_tts?ie=UTF-8&tl={lang}&client=tw-ob&q={Uri.EscapeDataString(text)}");
    }
}
