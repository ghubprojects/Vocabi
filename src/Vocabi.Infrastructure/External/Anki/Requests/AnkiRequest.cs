using System.Text.Json.Serialization;

namespace Vocabi.Infrastructure.External.Anki.Requests;

public sealed class AnkiRequest(string action, object? parameters = null) : AnkiBaseRequest
{
    [JsonPropertyName("action")]
    public override string Action { get; } = action;

    [JsonPropertyName("params")]
    public object Params { get; init; } = parameters ?? new { };
}