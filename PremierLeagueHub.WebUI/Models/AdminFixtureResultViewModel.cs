namespace PremierLeagueHub.WebUI.Models;

public class AdminFixtureResultViewModel
{
    public int FixtureId { get; set; }

    public string HomeTeamName { get; set; } = string.Empty;
    public string HomeTeamLogoUrl { get; set; } = string.Empty;

    public string AwayTeamName { get; set; } = string.Empty;
    public string AwayTeamLogoUrl { get; set; } = string.Empty;

    public DateTime MatchDate { get; set; }

    public string StadiumName { get; set; } = string.Empty;

    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    public string Status { get; set; } = "Finished";
}