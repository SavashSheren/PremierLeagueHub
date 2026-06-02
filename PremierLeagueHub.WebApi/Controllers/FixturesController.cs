using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.BusinessLayer.Abstract;
using PremierLeagueHub.DtoLayer.FixtureDtos;

namespace PremierLeagueHub.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FixturesController : ControllerBase
{
    private readonly IFixtureService _fixtureService;

    public FixturesController(IFixtureService fixtureService)
    {
        _fixtureService = fixtureService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFixtures()
    {
        var fixtures = await _fixtureService.GetAllFixturesAsync();
        return Ok(fixtures);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFixtureById(int id)
    {
        var fixture = await _fixtureService.GetFixtureByIdAsync(id);

        if (fixture == null)
        {
            return NotFound("Fixture not found.");
        }

        return Ok(fixture);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFixture(CreateFixtureDto createFixtureDto)
    {
        if (createFixtureDto.HomeTeamId == createFixtureDto.AwayTeamId)
        {
            return BadRequest("Home team and away team cannot be the same.");
        }

        await _fixtureService.CreateFixtureAsync(createFixtureDto);
        return Ok("Fixture created successfully.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateFixture(UpdateFixtureDto updateFixtureDto)
    {
        if (updateFixtureDto.HomeTeamId == updateFixtureDto.AwayTeamId)
        {
            return BadRequest("Home team and away team cannot be the same.");
        }

        var result = await _fixtureService.UpdateFixtureAsync(updateFixtureDto);

        if (!result)
        {
            return NotFound("Fixture not found.");
        }

        return Ok("Fixture updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFixture(int id)
    {
        var result = await _fixtureService.DeleteFixtureAsync(id);

        if (!result)
        {
            return NotFound("Fixture not found.");
        }

        return Ok("Fixture deleted successfully.");
    }
    [HttpPut("result")]
    public async Task<IActionResult> UpdateFixtureResult(UpdateFixtureResultDto updateFixtureResultDto)
    {
        if (updateFixtureResultDto.HomeScore < 0 || updateFixtureResultDto.AwayScore < 0)
        {
            return BadRequest("Scores cannot be negative.");
        }

        var result = await _fixtureService.UpdateFixtureResultAsync(updateFixtureResultDto);

        if (!result)
        {
            return NotFound("Fixture not found.");
        }

        return Ok("Fixture result updated successfully.");
    }
}