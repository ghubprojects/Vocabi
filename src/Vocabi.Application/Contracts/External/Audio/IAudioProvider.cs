using Vocabi.Application.Common.Models;

namespace Vocabi.Application.Contracts.External.Audio;

public interface IAudioProvider : IExternalProvider
{
    Task<Result<string>> GetAsync(string text, string lang = "en");
}