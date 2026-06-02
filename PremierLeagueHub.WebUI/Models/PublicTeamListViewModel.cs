using PremierLeagueHub.DtoLayer.FixtureDtos;
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

    public List<ResultFixtureDto> Fixtures { get; set; } = new List<ResultFixtureDto>();

    public ResultFixtureDto? FeaturedFixture =>
        Fixtures
            .Where(x => x.IsTopMatch)
            .OrderByDescending(x => x.HasResult)
            .ThenBy(x => x.MatchDate)
            .FirstOrDefault()
        ?? Fixtures
            .OrderBy(x => x.MatchDate)
            .FirstOrDefault();

    public List<ResultFixtureDto> LatestResults =>
        Fixtures
            .Where(x => x.HasResult || x.Status == "Finished")
            .OrderByDescending(x => x.MatchDate)
            .Take(4)
            .ToList();

    public List<ResultFixtureDto> UpcomingFixtures =>
        Fixtures
            .Where(x => !x.HasResult && x.Status != "Finished")
            .OrderBy(x => x.MatchDate)
            .Take(4)
            .ToList();

    public int FinishedMatchesCount =>
        Fixtures.Count(x => x.HasResult || x.Status == "Finished");

    public int TotalFixturesCount => Fixtures.Count;
}