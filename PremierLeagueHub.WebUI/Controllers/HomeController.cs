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
}