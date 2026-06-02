using PremierLeagueHub.DtoLayer.FixtureDtos;

namespace PremierLeagueHub.WebUI.Models;

public class PublicFixtureListViewModel
{
    public List<ResultFixtureDto> Fixtures { get; set; } = new List<ResultFixtureDto>();

    public int TotalFixtures => Fixtures.Count;

    public int TopMatchCount => Fixtures.Count(x => x.IsTopMatch);

    public int WeekNumber => Fixtures.FirstOrDefault()?.WeekNumber ?? 0;

    public string Season => Fixtures.FirstOrDefault()?.Season ?? "2025/2026";
}