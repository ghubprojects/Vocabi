using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Vocabi.Infrastructure.External.Anki.Requests;
using Vocabi.Infrastructure.External.Anki.Responses;

namespace Vocabi.Infrastructure.External.Anki;

public class AnkiConnectClient : IAnkiConnectClient
{
    private readonly HttpClient _http;

    public AnkiConnectClient(HttpClient http)
    {
        _http = http;
        _http.BaseAddress ??= new Uri("http://localhost:8765");
    }

    public Task<JsonElement?> InvokeAsync(AnkiRequest request)
        => SendAsync<JsonElement?>(request);

    public Task<List<AnkiResponse<JsonElement>>?> InvokeMultiAsync(params AnkiRequest[] requests)
        => SendAsync<List<AnkiResponse<JsonElement>>?>(new AnkiMultiRequest(requests));

    private async Task<T?> SendAsync<T>(AnkiBaseRequest request)
    {
        var json = JsonSerializer.Serialize(request, request.GetType());
        var response = await _http.PostAsync("", new StringContent(json, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();

        var envelope = await response.Content.ReadFromJsonAsync<AnkiResponse<T>>()
            ?? throw new InvalidOperationException("Invalid response from AnkiConnect (null).");

        if (!string.IsNullOrEmpty(envelope.Error))
            throw new InvalidOperationException($"AnkiConnect error: {envelope.Error}");

        return envelope.Result;
    }
}