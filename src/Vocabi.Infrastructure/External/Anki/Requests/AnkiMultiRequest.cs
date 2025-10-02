using System.Text.Json.Serialization;

namespace Vocabi.Infrastructure.External.Anki.Requests;

public sealed class AnkiMultiRequest(IEnumerable<AnkiRequest> requests) : AnkiBaseRequest
{
    [JsonPropertyName("action")]
    public override string Action => "multi";

    [JsonPropertyName("params")]
    public MultiParams Params { get; init; } = new MultiParams { Actions = [.. requests] };

    public sealed class MultiParams
    {
        [JsonPropertyName("actions")]
        public List<AnkiRequest> Actions { get; init; } = [];
    }
}