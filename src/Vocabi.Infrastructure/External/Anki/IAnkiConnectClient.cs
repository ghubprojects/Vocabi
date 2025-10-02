using System.Text.Json;
using Vocabi.Infrastructure.External.Anki.Requests;
using Vocabi.Infrastructure.External.Anki.Responses;

namespace Vocabi.Infrastructure.External.Anki;

public interface IAnkiConnectClient
{
    Task<JsonElement?> InvokeAsync(AnkiRequest request);
    Task<List<AnkiResponse<JsonElement>>?> InvokeMultiAsync(params AnkiRequest[] requests);
}