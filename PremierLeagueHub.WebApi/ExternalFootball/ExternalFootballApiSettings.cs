namespace PremierLeagueHub.WebApi.ExternalFootball;

public class ExternalFootballApiSettings
{
    public string Provider { get; set; } = "ApiFootball";

    public string BaseUrl { get; set; } = "https://v3.football.api-sports.io/";

    public string ApiKey { get; set; } = string.Empty;

    public int PremierLeagueId { get; set; } = 39;

    public int Season { get; set; } = 2025;
}