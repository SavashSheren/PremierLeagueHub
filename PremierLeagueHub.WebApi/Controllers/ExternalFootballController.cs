using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.WebApi.ExternalFootball;

namespace PremierLeagueHub.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExternalFootballController : ControllerBase
{
    private readonly IExternalFootballApiService _externalFootballApiService;

    public ExternalFootballController(IExternalFootballApiService externalFootballApiService)
    {
        _externalFootballApiService = externalFootballApiService;
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new
        {
            Provider = _externalFootballApiService.Provider,
            ApiKeyConfigured = _externalFootballApiService.IsConfigured,
            PremierLeagueId = _externalFootballApiService.PremierLeagueId,
            Season = _externalFootballApiService.Season,
            Message = _externalFootballApiService.IsConfigured
                ? "External football API is configured."
                : "External football API key is not configured. Use user-secrets or appsettings.Development.json."
        });
    }

    [HttpGet("fixtures-preview")]
    public async Task<IActionResult> GetFixturesPreview()
    {
        if (!_externalFootballApiService.IsConfigured)
        {
            return BadRequest(new
            {
                Message = "External football API key is not configured.",
                NextStep = "Set ExternalFootballApi:ApiKey using user-secrets."
            });
        }

        var rawJson = await _externalFootballApiService.GetPremierLeagueFixturesPreviewRawAsync();

        return Content(rawJson, "application/json");
    }
}