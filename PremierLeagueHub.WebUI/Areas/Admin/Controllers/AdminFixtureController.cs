using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.DtoLayer.FixtureDtos;
using PremierLeagueHub.WebUI.Models;
using System.Net.Http.Json;

namespace PremierLeagueHub.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class AdminFixtureController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AdminFixtureController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        var fixtures = await client.GetFromJsonAsync<List<ResultFixtureDto>>("Fixtures")
                       ?? new List<ResultFixtureDto>();

        fixtures = fixtures
            .OrderBy(x => x.MatchDate)
            .ToList();

        return View(fixtures);
    }

    [HttpGet]
    public async Task<IActionResult> EditResult(int id)
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        var fixture = await client.GetFromJsonAsync<GetFixtureByIdDto>($"Fixtures/{id}");

        if (fixture == null)
        {
            TempData["ErrorMessage"] = "Fixture not found.";
            return RedirectToAction(nameof(Index));
        }

        var model = new AdminFixtureResultViewModel
        {
            FixtureId = fixture.FixtureId,
            HomeTeamName = fixture.HomeTeamName,
            HomeTeamLogoUrl = fixture.HomeTeamLogoUrl,
            AwayTeamName = fixture.AwayTeamName,
            AwayTeamLogoUrl = fixture.AwayTeamLogoUrl,
            MatchDate = fixture.MatchDate,
            StadiumName = fixture.StadiumName,
            HomeScore = fixture.HomeScore ?? 0,
            AwayScore = fixture.AwayScore ?? 0,
            Status = string.IsNullOrWhiteSpace(fixture.Status) ? "Finished" : fixture.Status
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditResult(AdminFixtureResultViewModel model)
    {
        if (model.HomeScore < 0 || model.AwayScore < 0)
        {
            ModelState.AddModelError(string.Empty, "Scores cannot be negative.");
            return View(model);
        }

        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        var dto = new UpdateFixtureResultDto
        {
            FixtureId = model.FixtureId,
            HomeScore = model.HomeScore,
            AwayScore = model.AwayScore,
            Status = model.Status
        };

        var response = await client.PutAsJsonAsync("Fixtures/result", dto);

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Fixture result updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating fixture result.");
        return View(model);
    }
}