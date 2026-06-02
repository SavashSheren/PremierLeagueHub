using System.Net.Http.Headers;

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

        request.Headers.Add("x-apisports-key", ApiKey);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"External football API request failed. Status: {(int)response.StatusCode}. Response: {content}");
        }

        return content;
    }

    private string GetString(string key, string defaultValue)
    {
        var value = _configuration[key];

        return string.IsNullOrWhiteSpace(value)
            ? defaultValue
            : value;
    }

    private int GetInt(string key, int defaultValue)
    {
        var value = _configuration[key];

        return int.TryParse(value, out var parsedValue)
            ? parsedValue
            : defaultValue;
    }
}