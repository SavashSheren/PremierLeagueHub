using System.Net.Http.Headers;
using System.Text.Json;

namespace PremierLeagueHub.WebApi.ExternalFootball;

public class ApiFootballExternalFootballApiService : IExternalFootballApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ApiFootballExternalFootballApiService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey);

    public string Provider => GetString("ExternalFootballApi:Provider", "ApiFootball");

    public int PremierLeagueId => GetInt("ExternalFootballApi:PremierLeagueId", 39);

    public int Season => GetInt("ExternalFootballApi:Season", 2025);

    private string ApiKey => GetString("ExternalFootballApi:ApiKey", string.Empty);

    private string RapidApiHost => GetString("ExternalFootballApi:RapidApiHost", "api-football-v1.p.rapidapi.com");

    public async Task<string> GetPremierLeagueFixturesPreviewRawAsync()
    {
        if (!IsConfigured)
        {
            throw new InvalidOperationException("External football API key is not configured.");
        }

        var client = _httpClientFactory.CreateClient("ExternalFootballApi");

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"fixtures?league={PremierLeagueId}&season={Season}&next=10");

        AddAuthenticationHeaders(request);

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"External football API request failed. Status: {(int)response.StatusCode}. Response: {content}");
        }

        ThrowIfApiFootballReturnedError(content);

        return content;
    }

    private void AddAuthenticationHeaders(HttpRequestMessage request)
    {
        if (Provider.Equals("RapidApi", StringComparison.OrdinalIgnoreCase))
        {
            request.Headers.Add("x-rapidapi-key", ApiKey);
            request.Headers.Add("x-rapidapi-host", RapidApiHost);
            return;
        }

        request.Headers.Add("x-apisports-key", ApiKey);
    }

    private static void ThrowIfApiFootballReturnedError(string json)
    {
        using var document = JsonDocument.Parse(json);

        if (!document.RootElement.TryGetProperty("errors", out var errorsElement))
        {
            return;
        }

        var hasErrors = errorsElement.ValueKind switch
        {
            JsonValueKind.Object => errorsElement.EnumerateObject().Any(),
            JsonValueKind.Array => errorsElement.GetArrayLength() > 0,
            _ => false
        };

        if (!hasErrors)
        {
            return;
        }

        throw new HttpRequestException($"External football API returned an error: {errorsElement}");
    }

    private string GetString(string key, string defaultValue)
    {
        var value = _configuration[key];

        return string.IsNullOrWhiteSpace(value)
            ? defaultValue
            : value.Trim();
    }

    private int GetInt(string key, int defaultValue)
    {
        var value = _configuration[key];

        return int.TryParse(value, out var parsedValue)
            ? parsedValue
            : defaultValue;
    }
}