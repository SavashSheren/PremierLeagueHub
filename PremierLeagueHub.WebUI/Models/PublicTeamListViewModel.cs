using PremierLeagueHub.DtoLayer.TeamDtos;

namespace PremierLeagueHub.WebUI.Models;

public class PublicTeamListViewModel
{
    public string SearchTerm { get; set; } = string.Empty;
    public string StatusFilter { get; set; } = "all";
    public string CityFilter { get; set; } = "all";

    public int TotalTeams { get; set; }
    public int FilteredTeamsCount { get; set; }

    public List<string> Cities { get; set; } = new List<string>();
    public List<ResultTeamDto> Teams { get; set; } = new List<ResultTeamDto>();
}