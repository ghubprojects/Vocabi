using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Vocabi.Infrastructure.External.Flashcards;

public sealed record AnkiResponse<T>(T? Result, string? Error);

public interface IAnkiConnectClient
{
    Task<T?> InvokeAsync<T>(string action, object? parameters = null, CancellationToken cancellationToken = default);
}

public class AnkiConnectClient : IAnkiConnectClient
{
    private readonly HttpClient httpClient;

    public AnkiConnectClient(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri("http://localhost:8765");
    }

    public async Task<T?> InvokeAsync<T>(string action, object? parameters = null, CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            action,
            version = 6,
            @params = parameters ?? new { }
        };
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AnkiResponse<T>>(cancellationToken)
            ?? throw new InvalidOperationException("Invalid response from AnkiConnect (null).");

        if (!string.IsNullOrEmpty(result.Error))
            throw new InvalidOperationException($"AnkiConnect error: {result.Error}");

        return result.Result;
    }
}