using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.DtoLayer.FixtureDtos;
using PremierLeagueHub.DtoLayer.MatchEventDtos;
using PremierLeagueHub.WebUI.Models;
using System.Net.Http.Json;

namespace PremierLeagueHub.WebUI.Controllers;

public class FixturesController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FixturesController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        try
        {
            var fixtures = await client.GetFromJsonAsync<List<ResultFixtureDto>>("Fixtures")
                           ?? new List<ResultFixtureDto>();

            var model = new PublicFixtureListViewModel
            {
                Fixtures = fixtures
                    .OrderBy(x => x.MatchDate)
                    .ToList()
            };

            return View(model);
        }
        catch
        {
            ViewBag.ApiError = "The fixture service is currently unavailable.";
            return View(new PublicFixtureListViewModel());
        }
    }

    public async Task<IActionResult> Detail(int id)
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        try
        {
            var fixture = await client.GetFromJsonAsync<GetFixtureByIdDto>($"Fixtures/{id}");

            if (fixture == null)
            {
                TempData["ErrorMessage"] = "Fixture not found.";
                return RedirectToAction(nameof(Index));
            }

            var events = new List<ResultMatchEventDto>();

            try
            {
                events = await client.GetFromJsonAsync<List<ResultMatchEventDto>>($"MatchEvents/by-fixture/{id}")
                         ?? new List<ResultMatchEventDto>();
            }
            catch
            {
                TempData["ErrorMessage"] = "Match events could not be loaded.";
            }

            var model = new PublicFixtureDetailViewModel
            {
                Fixture = fixture,
                MatchEvents = events
                    .OrderBy(x => x.Minute)
                    .ToList()
            };

            return View(model);
        }
        catch
        {
            TempData["ErrorMessage"] = "The fixture detail could not be loaded.";
            return RedirectToAction(nameof(Index));
        }
    }
}