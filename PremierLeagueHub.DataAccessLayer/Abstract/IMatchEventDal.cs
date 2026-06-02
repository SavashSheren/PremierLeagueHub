using PremierLeagueHub.EntityLayer.Entities;

namespace PremierLeagueHub.DataAccessLayer.Abstract;

public interface IMatchEventDal : IGenericDal<MatchEvent>
{
    Task<List<MatchEvent>> GetAllMatchEventsWithTeamAsync();
    Task<List<MatchEvent>> GetMatchEventsByFixtureIdAsync(int fixtureId);
    Task<MatchEvent?> GetMatchEventWithTeamByIdAsync(int id);
}