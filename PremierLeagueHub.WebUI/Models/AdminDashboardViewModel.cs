using PremierLeagueHub.DtoLayer.FixtureDtos;
using PremierLeagueHub.DtoLayer.MatchEventDtos;
using PremierLeagueHub.DtoLayer.TeamDtos;

namespace PremierLeagueHub.WebUI.Models;

public class AdminDashboardViewModel
{
    public int TotalTeams { get; set; }
    public int ActiveTeams { get; set; }
    public int PassiveTeams { get; set; }
    public int TotalCities { get; set; }

    public int TotalFixtures { get; set; }
    public int FinishedFixtures { get; set; }
    public int ScheduledFixtures { get; set; }
    public int LiveFixtures { get; set; }
    public int TopMatchCount { get; set; }

    public int TotalMatchEvents { get; set; }
    public int GoalEvents { get; set; }
    public int CardEvents { get; set; }

    public List<ResultTeamDto> LatestTeams { get; set; } = new List<ResultTeamDto>();
    public List<ResultFixtureDto> LatestFixtures { get; set; } = new List<ResultFixtureDto>();
    public List<ResultMatchEventDto> LatestMatchEvents { get; set; } = new List<ResultMatchEventDto>();
}