using FluentResults;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using Vocabi.Application.Contracts.External.Image;

namespace Vocabi.Infrastructure.External.Image;

public class PixabayProvider(HttpClient httpClient, IOptions<PixabaySettings> options) : IImageProvider
{
    public string ProviderName => "Pixabay API";

    private readonly PixabaySettings _settings = options.Value;

    public async Task<Result<IReadOnlyList<string>>> GetAsync(string keyword, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = BuildRequest(keyword);
            var response = await httpClient.GetFromJsonAsync<PixabayResponse>(url, cancellationToken);

            if (response?.Hits == null || response.Hits.Count == 0)
                return Result.Fail("No images found");

            return Result.Ok<IReadOnlyList<string>>([.. response.Hits.Select(h => h.LargeImageURL)]);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    private string BuildRequest(string query)
    {
        var dict = new Dictionary<string, string?>
        {
            ["key"] = _settings.ApiKey,
            ["q"] = query,
            ["lang"] = _settings.Language,
            ["image_type"] = _settings.ImageType,
            ["per_page"] = _settings.PerPage.ToString()
        };

        return QueryHelpers.AddQueryString(_settings.BaseUrl, dict);
    }
}