using System.Text.Json.Serialization;

namespace Vocabi.Infrastructure.External.Anki.Requests;

public abstract class AnkiBaseRequest
{
    [JsonPropertyName("action")]
    public abstract string Action { get; }

    [JsonPropertyName("version")]
    public int Version { get; init; } = 6;
}