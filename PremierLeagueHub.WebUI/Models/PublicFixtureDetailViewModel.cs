using PremierLeagueHub.DtoLayer.FixtureDtos;
using PremierLeagueHub.DtoLayer.MatchEventDtos;

namespace PremierLeagueHub.WebUI.Models;

public class PublicFixtureDetailViewModel
{
    public GetFixtureByIdDto Fixture { get; set; } = new GetFixtureByIdDto();

    public List<ResultMatchEventDto> MatchEvents { get; set; } = new List<ResultMatchEventDto>();

    public bool HasEvents => MatchEvents.Any();
}