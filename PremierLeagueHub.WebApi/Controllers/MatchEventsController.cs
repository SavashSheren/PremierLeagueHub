using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.BusinessLayer.Abstract;
using PremierLeagueHub.DtoLayer.MatchEventDtos;

namespace PremierLeagueHub.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MatchEventsController : ControllerBase
{
    private readonly IMatchEventService _matchEventService;

    public MatchEventsController(IMatchEventService matchEventService)
    {
        _matchEventService = matchEventService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMatchEvents()
    {
        var matchEvents = await _matchEventService.GetAllMatchEventsAsync();
        return Ok(matchEvents);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMatchEventById(int id)
    {
        var matchEvent = await _matchEventService.GetMatchEventByIdAsync(id);

        if (matchEvent == null)
        {
            return NotFound("Match event not found.");
        }

        return Ok(matchEvent);
    }

    [HttpGet("by-fixture/{fixtureId}")]
    public async Task<IActionResult> GetMatchEventsByFixtureId(int fixtureId)
    {
        var matchEvents = await _matchEventService.GetMatchEventsByFixtureIdAsync(fixtureId);
        return Ok(matchEvents);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMatchEvent(CreateMatchEventDto createMatchEventDto)
    {
        var validationResult = ValidateMatchEvent(
            createMatchEventDto.FixtureId,
            createMatchEventDto.TeamId,
            createMatchEventDto.Minute,
            createMatchEventDto.EventType,
            createMatchEventDto.PlayerName);

        if (!string.IsNullOrWhiteSpace(validationResult))
        {
            return BadRequest(validationResult);
        }

        await _matchEventService.CreateMatchEventAsync(createMatchEventDto);
        return Ok("Match event created successfully.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMatchEvent(UpdateMatchEventDto updateMatchEventDto)
    {
        var validationResult = ValidateMatchEvent(
            updateMatchEventDto.FixtureId,
            updateMatchEventDto.TeamId,
            updateMatchEventDto.Minute,
            updateMatchEventDto.EventType,
            updateMatchEventDto.PlayerName);

        if (!string.IsNullOrWhiteSpace(validationResult))
        {
            return BadRequest(validationResult);
        }

        var result = await _matchEventService.UpdateMatchEventAsync(updateMatchEventDto);

        if (!result)
        {
            return NotFound("Match event not found.");
        }

        return Ok("Match event updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatchEvent(int id)
    {
        var result = await _matchEventService.DeleteMatchEventAsync(id);

        if (!result)
        {
            return NotFound("Match event not found.");
        }

        return Ok("Match event deleted successfully.");
    }

    private static string ValidateMatchEvent(
        int fixtureId,
        int teamId,
        int minute,
        string eventType,
        string playerName)
    {
        if (fixtureId <= 0)
        {
            return "FixtureId is required.";
        }

        if (teamId <= 0)
        {
            return "TeamId is required.";
        }

        if (minute < 0 || minute > 130)
        {
            return "Minute must be between 0 and 130.";
        }

        if (string.IsNullOrWhiteSpace(playerName))
        {
            return "Player name is required.";
        }

        var allowedEventTypes = new[]
        {
            "Goal",
            "YellowCard",
            "RedCard",
            "Substitution",
            "Penalty",
            "VAR"
        };

        var isValidEventType = allowedEventTypes.Any(x =>
            x.Equals(eventType, StringComparison.OrdinalIgnoreCase));

        if (!isValidEventType)
        {
            return "Invalid event type. Allowed values: Goal, YellowCard, RedCard, Substitution, Penalty, VAR.";
        }

        return string.Empty;
    }
}