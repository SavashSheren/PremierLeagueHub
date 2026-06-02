using PremierLeagueHub.DtoLayer.MatchEventDtos;

namespace PremierLeagueHub.BusinessLayer.Abstract;

public interface IMatchEventService
{
    Task<List<ResultMatchEventDto>> GetAllMatchEventsAsync();
    Task<List<ResultMatchEventDto>> GetMatchEventsByFixtureIdAsync(int fixtureId);
    Task<GetMatchEventByIdDto?> GetMatchEventByIdAsync(int id);
    Task CreateMatchEventAsync(CreateMatchEventDto createMatchEventDto);
    Task<bool> UpdateMatchEventAsync(UpdateMatchEventDto updateMatchEventDto);
    Task<bool> DeleteMatchEventAsync(int id);
}