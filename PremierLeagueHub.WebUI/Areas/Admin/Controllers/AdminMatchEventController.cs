using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.DtoLayer.FixtureDtos;
using PremierLeagueHub.DtoLayer.MatchEventDtos;
using PremierLeagueHub.WebUI.Models;
using System.Net.Http.Json;

namespace PremierLeagueHub.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class AdminMatchEventController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AdminMatchEventController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index(int fixtureId)
    {
        var model = await BuildMatchEventViewModelAsync(fixtureId);

        if (model == null)
        {
            TempData["ErrorMessage"] = "Fixture not found.";
            return RedirectToAction("Index", "AdminFixture", new { area = "Admin" });
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AdminMatchEventViewModel model)
    {
        if (model.FixtureId <= 0)
        {
            TempData["ErrorMessage"] = "Fixture is required.";
            return RedirectToAction("Index", "AdminFixture", new { area = "Admin" });
        }

        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        var fixture = await client.GetFromJsonAsync<GetFixtureByIdDto>($"Fixtures/{model.FixtureId}");

        if (fixture == null)
        {
            TempData["ErrorMessage"] = "Fixture not found.";
            return RedirectToAction("Index", "AdminFixture", new { area = "Admin" });
        }

        if (model.TeamId != fixture.HomeTeamId && model.TeamId != fixture.AwayTeamId)
        {
            TempData["ErrorMessage"] = "Selected team must be one of the fixture teams.";
            return RedirectToAction(nameof(Index), new { fixtureId = model.FixtureId });
        }

        if (model.Minute < 0 || model.Minute > 130)
        {
            TempData["ErrorMessage"] = "Minute must be between 0 and 130.";
            return RedirectToAction(nameof(Index), new { fixtureId = model.FixtureId });
        }

        if (string.IsNullOrWhiteSpace(model.PlayerName))
        {
            TempData["ErrorMessage"] = "Player name is required.";
            return RedirectToAction(nameof(Index), new { fixtureId = model.FixtureId });
        }

        var dto = new CreateMatchEventDto
        {
            FixtureId = model.FixtureId,
            TeamId = model.TeamId,
            Minute = model.Minute,
            EventType = model.EventType,
            PlayerName = model.PlayerName,
            AssistPlayerName = model.AssistPlayerName ?? string.Empty,
            RelatedPlayerName = model.RelatedPlayerName ?? string.Empty,
            Description = model.Description ?? string.Empty
        };

        var response = await client.PostAsJsonAsync("MatchEvents", dto);

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Match event added successfully.";
            return RedirectToAction(nameof(Index), new { fixtureId = model.FixtureId });
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        TempData["ErrorMessage"] = string.IsNullOrWhiteSpace(errorMessage)
            ? "Match event could not be added."
            : errorMessage;

        return RedirectToAction(nameof(Index), new { fixtureId = model.FixtureId });
    }

    public async Task<IActionResult> Delete(int id, int fixtureId)
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        var response = await client.DeleteAsync($"MatchEvents/{id}");

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Match event deleted successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Match event could not be deleted.";
        }

        return RedirectToAction(nameof(Index), new { fixtureId });
    }

    private async Task<AdminMatchEventViewModel?> BuildMatchEventViewModelAsync(int fixtureId)
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        var fixture = await client.GetFromJsonAsync<GetFixtureByIdDto>($"Fixtures/{fixtureId}");

        if (fixture == null)
        {
            return null;
        }

        var events = await client.GetFromJsonAsync<List<ResultMatchEventDto>>($"MatchEvents/by-fixture/{fixtureId}")
                     ?? new List<ResultMatchEventDto>();

        return new AdminMatchEventViewModel
        {
            FixtureId = fixture.FixtureId,

            HomeTeamId = fixture.HomeTeamId,
            HomeTeamName = fixture.HomeTeamName,
            HomeTeamLogoUrl = fixture.HomeTeamLogoUrl,

            AwayTeamId = fixture.AwayTeamId,
            AwayTeamName = fixture.AwayTeamName,
            AwayTeamLogoUrl = fixture.AwayTeamLogoUrl,

            MatchDate = fixture.MatchDate,
            StadiumName = fixture.StadiumName,

            TeamId = fixture.HomeTeamId,
            EventType = "Goal",

            MatchEvents = events
                .OrderBy(x => x.Minute)
                .ToList()
        };
    }
}