using Microsoft.EntityFrameworkCore;
using PremierLeagueHub.DataAccessLayer.Abstract;
using PremierLeagueHub.DataAccessLayer.Concrete;
using PremierLeagueHub.DataAccessLayer.Repositories;
using PremierLeagueHub.EntityLayer.Entities;

namespace PremierLeagueHub.DataAccessLayer.EntityFramework;

public class EfFixtureDal : GenericRepository<Fixture>, IFixtureDal
{
    private readonly PremierLeagueHubContext _context;

    public EfFixtureDal(PremierLeagueHubContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Fixture>> GetAllFixturesWithTeamsAsync()
    {
        return await _context.Fixtures
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .OrderBy(x => x.MatchDate)
            .ToListAsync();
    }

    public async Task<Fixture?> GetFixtureWithTeamsByIdAsync(int id)
    {
        return await _context.Fixtures
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .FirstOrDefaultAsync(x => x.FixtureId == id);
    }
}