using Microsoft.EntityFrameworkCore;
using PremierLeagueHub.DataAccessLayer.Abstract;
using PremierLeagueHub.DataAccessLayer.Concrete;
using PremierLeagueHub.DataAccessLayer.Repositories;
using PremierLeagueHub.EntityLayer.Entities;

namespace PremierLeagueHub.DataAccessLayer.EntityFramework;

public class EfMatchEventDal : GenericRepository<MatchEvent>, IMatchEventDal
{
    private readonly PremierLeagueHubContext _context;

    public EfMatchEventDal(PremierLeagueHubContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<MatchEvent>> GetAllMatchEventsWithTeamAsync()
    {
        return await _context.MatchEvents
            .Include(x => x.Team)
            .OrderBy(x => x.FixtureId)
            .ThenBy(x => x.Minute)
            .ToListAsync();
    }

    public async Task<List<MatchEvent>> GetMatchEventsByFixtureIdAsync(int fixtureId)
    {
        return await _context.MatchEvents
            .Include(x => x.Team)
            .Where(x => x.FixtureId == fixtureId)
            .OrderBy(x => x.Minute)
            .ToListAsync();
    }

    public async Task<MatchEvent?> GetMatchEventWithTeamByIdAsync(int id)
    {
        return await _context.MatchEvents
            .Include(x => x.Team)
            .FirstOrDefaultAsync(x => x.MatchEventId == id);
    }
}