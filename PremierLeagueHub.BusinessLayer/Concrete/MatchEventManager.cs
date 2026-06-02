using AutoMapper;
using PremierLeagueHub.BusinessLayer.Abstract;
using PremierLeagueHub.DataAccessLayer.Abstract;
using PremierLeagueHub.DtoLayer.MatchEventDtos;
using PremierLeagueHub.EntityLayer.Entities;

namespace PremierLeagueHub.BusinessLayer.Concrete;

public class MatchEventManager : IMatchEventService
{
    private readonly IMatchEventDal _matchEventDal;
    private readonly IMapper _mapper;

    public MatchEventManager(IMatchEventDal matchEventDal, IMapper mapper)
    {
        _matchEventDal = matchEventDal;
        _mapper = mapper;
    }

    public async Task<List<ResultMatchEventDto>> GetAllMatchEventsAsync()
    {
        var matchEvents = await _matchEventDal.GetAllMatchEventsWithTeamAsync();

        return matchEvents.Select(MapToResultMatchEventDto).ToList();
    }

    public async Task<List<ResultMatchEventDto>> GetMatchEventsByFixtureIdAsync(int fixtureId)
    {
        var matchEvents = await _matchEventDal.GetMatchEventsByFixtureIdAsync(fixtureId);

        return matchEvents.Select(MapToResultMatchEventDto).ToList();
    }

    public async Task<GetMatchEventByIdDto?> GetMatchEventByIdAsync(int id)
    {
        var matchEvent = await _matchEventDal.GetMatchEventWithTeamByIdAsync(id);

        if (matchEvent == null)
        {
            return null;
        }

        return MapToGetMatchEventByIdDto(matchEvent);
    }

    public async Task CreateMatchEventAsync(CreateMatchEventDto createMatchEventDto)
    {
        var matchEvent = _mapper.Map<MatchEvent>(createMatchEventDto);
        await _matchEventDal.InsertAsync(matchEvent);
    }

    public async Task<bool> UpdateMatchEventAsync(UpdateMatchEventDto updateMatchEventDto)
    {
        var existingMatchEvent = await _matchEventDal.GetByIdAsync(updateMatchEventDto.MatchEventId);

        if (existingMatchEvent == null)
        {
            return false;
        }

        _mapper.Map(updateMatchEventDto, existingMatchEvent);

        await _matchEventDal.UpdateAsync(existingMatchEvent);
        return true;
    }

    public async Task<bool> DeleteMatchEventAsync(int id)
    {
        var matchEvent = await _matchEventDal.GetByIdAsync(id);

        if (matchEvent == null)
        {
            return false;
        }

        await _matchEventDal.DeleteAsync(matchEvent);
        return true;
    }

    private static ResultMatchEventDto MapToResultMatchEventDto(MatchEvent matchEvent)
    {
        return new ResultMatchEventDto
        {
            MatchEventId = matchEvent.MatchEventId,
            FixtureId = matchEvent.FixtureId,
            TeamId = matchEvent.TeamId,
            TeamName = matchEvent.Team?.TeamName ?? string.Empty,
            TeamShortName = matchEvent.Team?.ShortName ?? string.Empty,
            TeamLogoUrl = matchEvent.Team?.LogoUrl ?? string.Empty,
            Minute = matchEvent.Minute,
            EventType = matchEvent.EventType,
            PlayerName = matchEvent.PlayerName,
            AssistPlayerName = matchEvent.AssistPlayerName,
            RelatedPlayerName = matchEvent.RelatedPlayerName,
            Description = matchEvent.Description
        };
    }

    private static GetMatchEventByIdDto MapToGetMatchEventByIdDto(MatchEvent matchEvent)
    {
        return new GetMatchEventByIdDto
        {
            MatchEventId = matchEvent.MatchEventId,
            FixtureId = matchEvent.FixtureId,
            TeamId = matchEvent.TeamId,
            TeamName = matchEvent.Team?.TeamName ?? string.Empty,
            TeamShortName = matchEvent.Team?.ShortName ?? string.Empty,
            TeamLogoUrl = matchEvent.Team?.LogoUrl ?? string.Empty,
            Minute = matchEvent.Minute,
            EventType = matchEvent.EventType,
            PlayerName = matchEvent.PlayerName,
            AssistPlayerName = matchEvent.AssistPlayerName,
            RelatedPlayerName = matchEvent.RelatedPlayerName,
            Description = matchEvent.Description
        };
    }
}