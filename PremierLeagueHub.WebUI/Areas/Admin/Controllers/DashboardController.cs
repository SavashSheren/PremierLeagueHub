using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.DtoLayer.FixtureDtos;
using PremierLeagueHub.DtoLayer.MatchEventDtos;
using PremierLeagueHub.DtoLayer.TeamDtos;
using PremierLeagueHub.WebUI.Models;
using System.Net.Http.Json;

namespace PremierLeagueHub.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DashboardController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        try
        {
            var teams = await client.GetFromJsonAsync<List<ResultTeamDto>>("Teams")
                        ?? new List<ResultTeamDto>();

            var fixtures = await client.GetFromJsonAsync<List<ResultFixtureDto>>("Fixtures")
                           ?? new List<ResultFixtureDto>();

            var matchEvents = await client.GetFromJsonAsync<List<ResultMatchEventDto>>("MatchEvents")
                              ?? new List<ResultMatchEventDto>();

            var model = new AdminDashboardViewModel
            {
                TotalTeams = teams.Count,
                ActiveTeams = teams.Count(x => x.IsActive),
                PassiveTeams = teams.Count(x => !x.IsActive),
                TotalCities = teams
                    .Select(x => x.City)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .Count(),

                TotalFixtures = fixtures.Count,
                FinishedFixtures = fixtures.Count(x => x.Status == "Finished" || x.HasResult),
                ScheduledFixtures = fixtures.Count(x => x.Status == "Scheduled" && !x.HasResult),
                LiveFixtures = fixtures.Count(x => x.Status == "Live"),
                TopMatchCount = fixtures.Count(x => x.IsTopMatch),

                TotalMatchEvents = matchEvents.Count,
                GoalEvents = matchEvents.Count(x =>
                    x.EventType == "Goal" || x.EventType == "Penalty"),
                CardEvents = matchEvents.Count(x =>
                    x.EventType == "YellowCard" || x.EventType == "RedCard"),

                LatestTeams = teams
                    .OrderByDescending(x => x.TeamId)
                    .Take(5)
                    .ToList(),

                LatestFixtures = fixtures
                    .OrderByDescending(x => x.MatchDate)
                    .Take(5)
                    .ToList(),

                LatestMatchEvents = matchEvents
                    .OrderByDescending(x => x.MatchEventId)
                    .Take(6)
                    .ToList()
            };

            return View(model);
        }
        catch
        {
            ViewBag.ApiError = "Dashboard data could not be loaded. Please check WebApi service.";
            return View(new AdminDashboardViewModel());
        }
    }
}