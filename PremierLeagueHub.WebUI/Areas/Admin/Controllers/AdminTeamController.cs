using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.DtoLayer.TeamDtos;
using PremierLeagueHub.WebUI.Models;
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

    [HttpGet]
    public IActionResult CreateTeam()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeam(CreateTeamDto createTeamDto)
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");
        var response = await client.PostAsJsonAsync("Teams", createTeamDto);

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Team created successfully.";
            return RedirectToAction(nameof(Index));
        }

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            await AddApiErrorsToModelState(response);
            return View(createTeamDto);
        }

        ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the team.");
        return View(createTeamDto);
    }

    [HttpGet]
    public async Task<IActionResult> EditTeam(int id)
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        var team = await client.GetFromJsonAsync<UpdateTeamDto>($"Teams/{id}");

        if (team == null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(team);
    }

    [HttpPost]
    public async Task<IActionResult> EditTeam(UpdateTeamDto updateTeamDto)
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");
        var response = await client.PutAsJsonAsync("Teams", updateTeamDto);

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Team updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            await AddApiErrorsToModelState(response);
            return View(updateTeamDto);
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            ModelState.AddModelError(string.Empty, "Team not found.");
            return View(updateTeamDto);
        }

        ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating the team.");
        return View(updateTeamDto);
    }

    private async Task AddApiErrorsToModelState(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(responseContent))
        {
            ModelState.AddModelError(string.Empty, "Validation failed.");
            return;
        }

        try
        {
            var validationErrors = System.Text.Json.JsonSerializer.Deserialize<List<ApiValidationError>>(
                responseContent,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (validationErrors != null)
            {
                foreach (var error in validationErrors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return;
            }
        }
        catch
        {
            // API may return a different validation format.
        }

        ModelState.AddModelError(string.Empty, responseContent);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteTeam(int id)
    {
        var client = _httpClientFactory.CreateClient("PremierLeagueApi");

        var response = await client.DeleteAsync($"Teams/{id}");

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Team deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            TempData["ErrorMessage"] = "Team not found.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = "An unexpected error occurred while deleting the team.";
        return RedirectToAction(nameof(Index));
    }
}