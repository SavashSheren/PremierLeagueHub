using PremierLeagueHub.EntityLayer.Entities;

namespace PremierLeagueHub.DataAccessLayer.Abstract;

public interface IFixtureDal : IGenericDal<Fixture>
{
    Task<List<Fixture>> GetAllFixturesWithTeamsAsync();
    Task<Fixture?> GetFixtureWithTeamsByIdAsync(int id);
}