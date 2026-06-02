namespace PremierLeagueHub.EntityLayer.Entities;

public class Fixture
{
    public int FixtureId { get; set; }

    public int HomeTeamId { get; set; }
    public Team? HomeTeam { get; set; }

    public int AwayTeamId { get; set; }
    public Team? AwayTeam { get; set; }

    public DateTime MatchDate { get; set; }

    public int WeekNumber { get; set; }

    public string Season { get; set; } = string.Empty;

    public string StadiumName { get; set; } = string.Empty;

    public bool IsTopMatch { get; set; }

    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }

    public string Status { get; set; } = "Scheduled";
}