using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.DtoLayer.TeamDtos;
using System.Net.Http.Json;

namespace PremierLeagueHub.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class AdminTeamController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AdminTeamController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        var teams = await client.GetFromJsonAsync<List<ResultTeamDto>>("Teams");

        return View(teams ?? new List<ResultTeamDto>());
    }
}