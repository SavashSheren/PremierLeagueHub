using PremierLeagueHub.DataAccessLayer.Abstract;
using PremierLeagueHub.DataAccessLayer.Concrete;
using PremierLeagueHub.DataAccessLayer.Repositories;
using PremierLeagueHub.EntityLayer.Entities;

namespace PremierLeagueHub.DataAccessLayer.EntityFramework;

public class EfTeamDal : GenericRepository<Team>, ITeamDal
{
    public EfTeamDal(PremierLeagueHubContext context) : base(context)
    {
    }
}