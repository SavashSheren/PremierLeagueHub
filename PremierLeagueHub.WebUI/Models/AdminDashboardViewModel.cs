using PremierLeagueHub.DtoLayer.TeamDtos;

namespace PremierLeagueHub.WebUI.Models;

public class AdminDashboardViewModel
{
    public int TotalTeams { get; set; }
    public int ActiveTeams { get; set; }
    public int PassiveTeams { get; set; }
    public int TotalCities { get; set; }

    public List<ResultTeamDto> LatestTeams { get; set; } = new List<ResultTeamDto>();
}