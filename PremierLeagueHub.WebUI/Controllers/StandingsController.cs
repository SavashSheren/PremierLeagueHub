using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.DtoLayer.TeamDtos;
using PremierLeagueHub.WebUI.Models;
using System.Net.Http.Json;

namespace PremierLeagueHub.WebUI.Controllers;

public class StandingsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public StandingsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        var teams = await client.GetFromJsonAsync<List<ResultTeamDto>>("Teams")
                    ?? new List<ResultTeamDto>();

        var rows = BuildDemoStandings(teams)
            .OrderByDescending(x => x.Points)
            .ThenByDescending(x => x.GoalDifference)
            .ThenByDescending(x => x.GoalsFor)
            .ToList();

        for (var i = 0; i < rows.Count; i++)
        {
            rows[i].Position = i + 1;
            rows[i].Zone = GetZone(i + 1, rows.Count);
        }

        var model = new StandingsViewModel
        {
            Rows = rows
        };

        return View(model);
    }

    private static List<StandingRowViewModel> BuildDemoStandings(List<ResultTeamDto> teams)
    {
        var demoStats = new Dictionary<string, (int played, int won, int drawn, int lost, int gf, int ga, List<string> form)>
        {
            ["LIV"] = (33, 24, 7, 2, 74, 30, new() { "W", "W", "D", "W", "W" }),
            ["ARS"] = (33, 22, 6, 5, 68, 26, new() { "D", "W", "W", "L", "W" }),
            ["MCI"] = (33, 20, 7, 6, 82, 44, new() { "W", "W", "W", "D", "W" }),
            ["CHE"] = (33, 18, 8, 7, 60, 42, new() { "L", "W", "D", "W", "W" }),
            ["NEW"] = (33, 17, 9, 7, 58, 38, new() { "D", "D", "W", "W", "D" }),
            ["AVL"] = (33, 17, 7, 9, 62, 48, new() { "W", "L", "W", "W", "W" }),
            ["TOT"] = (33, 15, 8, 10, 56, 50, new() { "D", "D", "L", "W", "D" }),
            ["MUN"] = (33, 13, 9, 11, 40, 46, new() { "L", "D", "L", "D", "L" }),
            ["WHU"] = (33, 12, 10, 11, 48, 52, new() { "L", "W", "D", "W", "L" }),
            ["BHA"] = (33, 12, 9, 12, 50, 54, new() { "W", "D", "W", "L", "D" }),
            ["BRE"] = (33, 11, 10, 12, 46, 52, new() { "W", "D", "L", "D", "L" }),
            ["FUL"] = (33, 11, 9, 13, 44, 55, new() { "D", "L", "W", "D", "W" }),
            ["WOL"] = (33, 11, 8, 14, 46, 58, new() { "W", "W", "L", "L", "D" }),
            ["EVE"] = (33, 10, 10, 13, 38, 52, new() { "W", "D", "D", "L", "W" }),
            ["CRY"] = (33, 9, 9, 15, 34, 50, new() { "L", "D", "L", "W", "D" }),
            ["NFO"] = (33, 9, 7, 17, 38, 58, new() { "D", "L", "W", "L", "D" }),
            ["BOU"] = (33, 8, 9, 16, 42, 62, new() { "D", "L", "D", "W", "D" }),
            ["LEE"] = (33, 7, 8, 18, 36, 68, new() { "W", "L", "L", "D", "L" }),
            ["SUN"] = (33, 5, 10, 18, 28, 64, new() { "D", "D", "L", "L", "L" }),
            ["BUR"] = (33, 3, 7, 23, 24, 74, new() { "L", "L", "D", "L", "L" })
        };

        var rows = new List<StandingRowViewModel>();

        foreach (var team in teams)
        {
            var shortName = team.ShortName.ToUpper();

            if (!demoStats.TryGetValue(shortName, out var stats))
            {
                stats = (33, 10, 8, 15, 38, 50, new() { "D", "L", "W", "D", "L" });
            }

            rows.Add(new StandingRowViewModel
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                ShortName = team.ShortName,
                LogoUrl = team.LogoUrl,
                Played = stats.played,
                Won = stats.won,
                Drawn = stats.drawn,
                Lost = stats.lost,
                GoalsFor = stats.gf,
                GoalsAgainst = stats.ga,
                Form = stats.form
            });
        }

        return rows;
    }

    private static string GetZone(int position, int totalTeams)
    {
        if (position == 1)
        {
            return "champion";
        }

        if (position <= 4)
        {
            return "ucl";
        }

        if (position <= 6)
        {
            return "uel";
        }

        if (position > totalTeams - 3)
        {
            return "relegation";
        }

        return "none";
    }
}