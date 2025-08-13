using System.Net.Http;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Audio;
using Vocabi.Application.Contracts.Storage;

namespace Vocabi.Infrastructure.External.GoogleTts;

public class GoogleTtsProvider(HttpClient httpClient, IFileStorage fileStorage) : IAudioProvider
{
    public string ProviderName => "Google TTS";

    public async Task<Result<string>> GetAsync(string text, string lang = "en")
    {
        var url = $"https://translate.google.com/translate_tts?ie=UTF-8&tl={lang}&client=tw-ob&q={Uri.EscapeDataString(text)}";
        var audioBytes = await httpClient.GetByteArrayAsync(url);

        // Lưu vào local storage
        var fileName = $"{Guid.NewGuid()}.mp3";
        await fileStorage.SaveAsync(fileName, audioBytes, "audio/mpeg");

        // Trả URL hoặc path tới file
        return $"/media/{fileName}";
    }
}
