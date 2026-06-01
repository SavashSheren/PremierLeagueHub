namespace PremierLeagueHub.WebUI.Models;

public class ApiValidationError
{
    public string PropertyName { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}