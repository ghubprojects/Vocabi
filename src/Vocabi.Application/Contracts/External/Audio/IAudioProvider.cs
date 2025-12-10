using FluentResults;

namespace Vocabi.Application.Contracts.External.Audio;

public interface IAudioProvider : IExternalProvider
{
    Result<string> Get(string text, string lang = "en");
}