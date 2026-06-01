using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.DtoLayer.TeamDtos;
using System.Net.Http.Json;

namespace PremierLeagueHub.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
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

            return View(teams);
        }
        catch
        {
            ViewBag.ApiError = "The API service is currently unavailable.";
            return View(new List<ResultTeamDto>());
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public async Task<IActionResult> TeamDetail(int id)
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        try
        {
            var team = await client.GetFromJsonAsync<GetTeamByIdDto>($"Teams/{id}");

            if (team == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(team);
        }
        catch
        {
            TempData["ErrorMessage"] = "The team detail could not be loaded.";
            return RedirectToAction(nameof(Index));
        }
    }
}