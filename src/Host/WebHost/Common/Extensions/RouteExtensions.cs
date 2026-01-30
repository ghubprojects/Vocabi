using System.Web;

namespace Vocabi.Web.Common.Extensions;

public static class RouteExtensions
{
    /// <summary>
    /// Thêm query string từ dictionary vào base URL.
    /// </summary>
    public static string WithQuery(this string baseUrl, IDictionary<string, object> queryParams)
    {
        if (queryParams == null || !queryParams.Any())
            return baseUrl;

        var query = string.Join("&", queryParams
            .Where(kv => kv.Value != null)
            .Select(kv => $"{HttpUtility.UrlEncode(kv.Key)}={HttpUtility.UrlEncode(kv.Value.ToString())}"));

        return $"{baseUrl}?{query}";
    }

    /// <summary>
    /// Thêm query string từ anonymous object vào base URL.
    /// </summary>
    public static string WithQuery(this string baseUrl, object queryParams)
    {
        if (queryParams == null)
            return baseUrl;

        var dict = queryParams.GetType()
            .GetProperties()
            .ToDictionary(
                p => p.Name,
                p => p.GetValue(queryParams, null)
            );

        return baseUrl.WithQuery(dict);
    }
}