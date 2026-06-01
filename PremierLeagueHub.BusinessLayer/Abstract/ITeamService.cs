using PremierLeagueHub.DtoLayer.TeamDtos;

namespace PremierLeagueHub.BusinessLayer.Abstract;

public interface ITeamService
{
    Task<List<ResultTeamDto>> GetAllTeamsAsync();
    Task<GetTeamByIdDto?> GetTeamByIdAsync(int id);
    Task CreateTeamAsync(CreateTeamDto createTeamDto);
    Task<bool> UpdateTeamAsync(UpdateTeamDto updateTeamDto);
    Task<bool> DeleteTeamAsync(int id);
}