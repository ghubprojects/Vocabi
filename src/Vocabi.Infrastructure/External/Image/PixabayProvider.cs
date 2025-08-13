using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Vocabi.Application.Common.Models;
using Vocabi.Application.Contracts.External.Image;

namespace Vocabi.Infrastructure.External.Image;

public class PixabayProvider(HttpClient httpClient, IConfiguration configuration) : IImageProvider
{
    public string ProviderName => "Pixabay API";
    
    private readonly string _apiKey = configuration["Pixabay:ApiKey"] 
        ?? throw new ArgumentNullException("Pixabay API key not configured");

    public async Task<Result<IReadOnlyList<string>>> GetAsync(string keyword, int limit = 5, CancellationToken cancellationToken = default)
    {
        var url = $"https://pixabay.com/api/?key={_apiKey}&q={Uri.EscapeDataString(keyword)}&per_page={limit}&image_type=photo";
        var response = await httpClient.GetFromJsonAsync<PixabayResponse>(url, cancellationToken);

        if (response?.Hits == null || response.Hits.Count == 0)
            return Result<IReadOnlyList<string>>.Failure("No images found");

        return Result<IReadOnlyList<string>>.Success([.. response.Hits.Select(h => h.LargeImageURL)]);
    }

    private class PixabayResponse
    {
        public List<PixabayHit> Hits { get; set; } = new();
    }

    private class PixabayHit
    {
        public string LargeImageURL { get; set; } = string.Empty;
    }
}