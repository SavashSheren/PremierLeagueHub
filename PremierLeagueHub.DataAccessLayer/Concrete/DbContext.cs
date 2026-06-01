using Microsoft.EntityFrameworkCore;
using PremierLeagueHub.EntityLayer.Entities;

namespace PremierLeagueHub.DataAccessLayer.Concreate;

public class PremierLeagueHubContext : DbContext
{
    public PremierLeagueHubContext(DbContextOptions<PremierLeagueHubContext> options)
        : base(options)
    {
    }

    public DbSet<Team> Teams { get; set; }
}