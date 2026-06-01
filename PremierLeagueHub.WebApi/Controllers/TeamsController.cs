using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.BusinessLayer.Abstract;
using PremierLeagueHub.DtoLayer.TeamDtos;

namespace PremierLeagueHub.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IValidator<CreateTeamDto> _createTeamValidator;
    private readonly IValidator<UpdateTeamDto> _updateTeamValidator;

    public TeamsController(
        ITeamService teamService,
        IValidator<CreateTeamDto> createTeamValidator,
        IValidator<UpdateTeamDto> updateTeamValidator)
    {
        _teamService = teamService;
        _createTeamValidator = createTeamValidator;
        _updateTeamValidator = updateTeamValidator;
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
        var validationResult = await _createTeamValidator.ValidateAsync(createTeamDto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => new
            {
                x.PropertyName,
                x.ErrorMessage
            });

            return BadRequest(errors);
        }

        await _teamService.CreateTeamAsync(createTeamDto);
        return Ok("Team created successfully.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTeam(UpdateTeamDto updateTeamDto)
    {
        var validationResult = await _updateTeamValidator.ValidateAsync(updateTeamDto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => new
            {
                x.PropertyName,
                x.ErrorMessage
            });

            return BadRequest(errors);
        }

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