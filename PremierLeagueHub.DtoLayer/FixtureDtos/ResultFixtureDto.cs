namespace PremierLeagueHub.DtoLayer.FixtureDtos;

public class ResultFixtureDto
{
    public int FixtureId { get; set; }

    public int HomeTeamId { get; set; }
    public string HomeTeamName { get; set; } = string.Empty;
    public string HomeTeamShortName { get; set; } = string.Empty;
    public string HomeTeamLogoUrl { get; set; } = string.Empty;

    public int AwayTeamId { get; set; }
    public string AwayTeamName { get; set; } = string.Empty;
    public string AwayTeamShortName { get; set; } = string.Empty;
    public string AwayTeamLogoUrl { get; set; } = string.Empty;

    public DateTime MatchDate { get; set; }

    public int WeekNumber { get; set; }

    public string Season { get; set; } = string.Empty;

    public string StadiumName { get; set; } = string.Empty;

    public bool IsTopMatch { get; set; }

    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }

    public bool HasResult => HomeScore.HasValue && AwayScore.HasValue;

    public string Status { get; set; } = string.Empty;
}