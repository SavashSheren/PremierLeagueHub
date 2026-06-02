namespace PremierLeagueHub.WebApi.ExternalFootball;

public interface IExternalFootballApiService
{
    bool IsConfigured { get; }

    string Provider { get; }

    int PremierLeagueId { get; }

    int Season { get; }

    Task<string> GetPremierLeagueFixturesPreviewRawAsync();
}