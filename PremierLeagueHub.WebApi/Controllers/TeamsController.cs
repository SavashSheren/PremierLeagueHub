using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.BusinessLayer.Abstract;
using PremierLeagueHub.DtoLayer.TeamDtos;

namespace PremierLeagueHub.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeams()
    {
        var teams = await _teamService.GetAllTeamsAsync();
        return Ok(teams);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeamById(int id)
    {
        var team = await _teamService.GetTeamByIdAsync(id);

        if (team == null)
        {
            return NotFound("Team not found.");
        }

        return Ok(team);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeam(CreateTeamDto createTeamDto)
    {
        await _teamService.CreateTeamAsync(createTeamDto);
        return Ok("Team created successfully.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTeam(UpdateTeamDto updateTeamDto)
    {
        var result = await _teamService.UpdateTeamAsync(updateTeamDto);

        if (!result)
        {
            return NotFound("Team not found.");
        }

        return Ok("Team updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeam(int id)
    {
        var result = await _teamService.DeleteTeamAsync(id);

        if (!result)
        {
            return NotFound("Team not found.");
        }

        return Ok("Team deleted successfully.");
    }
}