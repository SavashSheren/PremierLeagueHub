using AutoMapper;
using PremierLeagueHub.BusinessLayer.Abstract;
using PremierLeagueHub.DataAccessLayer.Abstract;
using PremierLeagueHub.DtoLayer.TeamDtos;
using PremierLeagueHub.EntityLayer.Entities;

namespace PremierLeagueHub.BusinessLayer.Concrete;

public class TeamManager : ITeamService
{
    private readonly ITeamDal _teamDal;
    private readonly IMapper _mapper;

    public TeamManager(ITeamDal teamDal, IMapper mapper)
    {
        _teamDal = teamDal;
        _mapper = mapper;
    }

    public async Task<List<ResultTeamDto>> GetAllTeamsAsync()
    {
        var teams = await _teamDal.GetAllAsync();
        return _mapper.Map<List<ResultTeamDto>>(teams);
    }

    public async Task<GetTeamByIdDto?> GetTeamByIdAsync(int id)
    {
        var team = await _teamDal.GetByIdAsync(id);

        if (team == null)
        {
            return null;
        }

        return _mapper.Map<GetTeamByIdDto>(team);
    }

    public async Task CreateTeamAsync(CreateTeamDto createTeamDto)
    {
        var team = _mapper.Map<Team>(createTeamDto);
        await _teamDal.InsertAsync(team);
    }

    public async Task<bool> UpdateTeamAsync(UpdateTeamDto updateTeamDto)
    {
        var existingTeam = await _teamDal.GetByIdAsync(updateTeamDto.TeamId);

        if (existingTeam == null)
        {
            return false;
        }

        _mapper.Map(updateTeamDto, existingTeam);

        await _teamDal.UpdateAsync(existingTeam);
        return true;
    }

    public async Task<bool> DeleteTeamAsync(int id)
    {
        var team = await _teamDal.GetByIdAsync(id);

        if (team == null)
        {
            return false;
        }

        await _teamDal.DeleteAsync(team);
        return true;
    }
}