using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.DtoLayer.TeamDtos;
using PremierLeagueHub.WebUI.Models;
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
    public async Task<IActionResult> Index(string? searchTerm, string statusFilter = "all", string cityFilter = "all")
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        try
        {
            var teams = await client.GetFromJsonAsync<List<ResultTeamDto>>("Teams")
                        ?? new List<ResultTeamDto>();

            var query = teams.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearch = searchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.TeamName.ToLower().Contains(normalizedSearch) ||
                    x.ShortName.ToLower().Contains(normalizedSearch) ||
                    x.City.ToLower().Contains(normalizedSearch) ||
                    x.StadiumName.ToLower().Contains(normalizedSearch) ||
                    x.ManagerName.ToLower().Contains(normalizedSearch));
            }

            if (statusFilter == "active")
            {
                query = query.Where(x => x.IsActive);
            }
            else if (statusFilter == "passive")
            {
                query = query.Where(x => !x.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(cityFilter) && cityFilter != "all")
            {
                query = query.Where(x => x.City == cityFilter);
            }

            var filteredTeams = query
                .OrderBy(x => x.TeamName)
                .ToList();

            var model = new PublicTeamListViewModel
            {
                SearchTerm = searchTerm ?? string.Empty,
                StatusFilter = statusFilter,
                CityFilter = cityFilter,
                TotalTeams = teams.Count,
                FilteredTeamsCount = filteredTeams.Count,
                Cities = teams
                    .Select(x => x.City)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList(),
                Teams = filteredTeams
            };

            return View(model);
        }
        catch
        {
            ViewBag.ApiError = "The API service is currently unavailable.";

            return View(new PublicTeamListViewModel());
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