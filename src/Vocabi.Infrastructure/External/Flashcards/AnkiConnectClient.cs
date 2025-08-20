using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Vocabi.Infrastructure.External.Flashcards;

public class AnkiResponse<T>
{
    public T? Result { get; set; }
    public string? Error { get; set; }
}

public interface IAnkiConnectClient
{
    Task<AnkiResponse<T>> InvokeAsync<T>(string action, object parameters, CancellationToken cancellationToken);
}

public class AnkiConnectClient(IHttpClientFactory httpClientFactory) : IAnkiConnectClient
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient();

    public async Task<AnkiResponse<T>> InvokeAsync<T>(string action, object parameters, CancellationToken cancellationToken)
    {
        var payload = new
        {
            action,
            version = 6,
            @params = parameters
        };
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("http://127.0.0.1:8765", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);

        var result = new AnkiResponse<T>();

        if (responseContent.TryGetProperty("error", out var errorProp) && errorProp.ValueKind != JsonValueKind.Null)
            result.Error = errorProp.GetString();

        if (responseContent.TryGetProperty("result", out var resultProp) && resultProp.ValueKind != JsonValueKind.Null)
            result.Result = resultProp.Deserialize<T>();

        return result;
    }
}