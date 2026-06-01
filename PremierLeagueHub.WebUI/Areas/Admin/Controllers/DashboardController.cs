using Microsoft.AspNetCore.Mvc;
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

        var teams = await client.GetFromJsonAsync<List<ResultTeamDto>>("Teams")
                    ?? new List<ResultTeamDto>();

        var model = new AdminDashboardViewModel
        {
            TotalTeams = teams.Count,
            ActiveTeams = teams.Count(x => x.IsActive),
            PassiveTeams = teams.Count(x => !x.IsActive),
            TotalCities = teams.Select(x => x.City).Distinct().Count(),
            LatestTeams = teams
                .OrderByDescending(x => x.TeamId)
                .Take(5)
                .ToList()
        };

        return View(model);
    }
}