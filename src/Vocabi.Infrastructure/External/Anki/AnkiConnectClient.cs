using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Vocabi.Infrastructure.External.Anki.Requests;
using Vocabi.Infrastructure.External.Anki.Responses;

namespace Vocabi.Infrastructure.External.Anki;

public class AnkiConnectClient : IAnkiConnectClient
{
    private readonly HttpClient httpClient;

    public AnkiConnectClient(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri("http://localhost:8765");
    }

    public async Task<JsonElement?> InvokeAsync(AnkiRequest request)
        => await SendAsync<JsonElement?>(request);

    public async Task<List<AnkiResponse<JsonElement>>?> InvokeMultiAsync(params AnkiRequest[] requests)
        => await SendAsync<List<AnkiResponse<JsonElement>>?>(new AnkiMultiRequest(requests));

    private async Task<T?> SendAsync<T>(AnkiBaseRequest request)
    {
        var json = JsonSerializer.Serialize(request, request.GetType());
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("", content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AnkiResponse<T>>()
            ?? throw new InvalidOperationException("Invalid response from AnkiConnect (null).");

        if (!string.IsNullOrEmpty(result.Error))
            throw new InvalidOperationException($"AnkiConnect error: {result.Error}");

        return result.Result;
    }
}