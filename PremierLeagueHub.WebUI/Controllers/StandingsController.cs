using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.DtoLayer.FixtureDtos;
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

        try
        {
            var teams = await client.GetFromJsonAsync<List<ResultTeamDto>>("Teams")
                        ?? new List<ResultTeamDto>();

            var fixtures = await client.GetFromJsonAsync<List<ResultFixtureDto>>("Fixtures")
                           ?? new List<ResultFixtureDto>();

            var finishedFixtures = fixtures
                .Where(x => x.HasResult || x.Status == "Finished")
                .Where(x => x.HomeScore.HasValue && x.AwayScore.HasValue)
                .OrderBy(x => x.MatchDate)
                .ToList();

            var rows = BuildDynamicStandings(teams, finishedFixtures)
                .OrderByDescending(x => x.Points)
                .ThenByDescending(x => x.GoalDifference)
                .ThenByDescending(x => x.GoalsFor)
                .ThenBy(x => x.TeamName)
                .ToList();

            for (var i = 0; i < rows.Count; i++)
            {
                rows[i].Position = i + 1;
                rows[i].Zone = GetZone(i + 1, rows.Count);
            }

            var model = new StandingsViewModel
            {
                Rows = rows,
                FinishedFixturesCount = finishedFixtures.Count,
                ScheduledFixturesCount = fixtures.Count(x => !x.HasResult && x.Status != "Finished")
            };

            return View(model);
        }
        catch
        {
            ViewBag.ApiError = "Standings data could not be loaded. Please check WebApi service.";

            return View(new StandingsViewModel());
        }
    }

    private static List<StandingRowViewModel> BuildDynamicStandings(
        List<ResultTeamDto> teams,
        List<ResultFixtureDto> finishedFixtures)
    {
        var rows = teams
            .Select(team => new StandingRowViewModel
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                ShortName = team.ShortName,
                LogoUrl = team.LogoUrl,
                Played = 0,
                Won = 0,
                Drawn = 0,
                Lost = 0,
                GoalsFor = 0,
                GoalsAgainst = 0,
                Form = new List<string>()
            })
            .ToDictionary(x => x.TeamId);

        var formHistory = teams.ToDictionary(
            x => x.TeamId,
            _ => new List<(DateTime MatchDate, string Result)>());

        foreach (var fixture in finishedFixtures)
        {
            if (!rows.ContainsKey(fixture.HomeTeamId) || !rows.ContainsKey(fixture.AwayTeamId))
            {
                continue;
            }

            var homeRow = rows[fixture.HomeTeamId];
            var awayRow = rows[fixture.AwayTeamId];

            var homeScore = fixture.HomeScore ?? 0;
            var awayScore = fixture.AwayScore ?? 0;

            homeRow.Played++;
            awayRow.Played++;

            homeRow.GoalsFor += homeScore;
            homeRow.GoalsAgainst += awayScore;

            awayRow.GoalsFor += awayScore;
            awayRow.GoalsAgainst += homeScore;

            if (homeScore > awayScore)
            {
                homeRow.Won++;
                awayRow.Lost++;

                formHistory[fixture.HomeTeamId].Add((fixture.MatchDate, "W"));
                formHistory[fixture.AwayTeamId].Add((fixture.MatchDate, "L"));
            }
            else if (homeScore < awayScore)
            {
                awayRow.Won++;
                homeRow.Lost++;

                formHistory[fixture.AwayTeamId].Add((fixture.MatchDate, "W"));
                formHistory[fixture.HomeTeamId].Add((fixture.MatchDate, "L"));
            }
            else
            {
                homeRow.Drawn++;
                awayRow.Drawn++;

                formHistory[fixture.HomeTeamId].Add((fixture.MatchDate, "D"));
                formHistory[fixture.AwayTeamId].Add((fixture.MatchDate, "D"));
            }
        }

        foreach (var row in rows.Values)
        {
            row.Form = formHistory[row.TeamId]
                .OrderByDescending(x => x.MatchDate)
                .Take(5)
                .Reverse()
                .Select(x => x.Result)
                .ToList();
        }

        return rows.Values.ToList();
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