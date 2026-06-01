namespace PremierLeagueHub.DtoLayer.TeamDtos;

public class ResultTeamDto
{
    public int TeamId { get; set; }

    public string TeamName { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;
    public string StadiumName { get; set; } = string.Empty;
    public string ManagerName { get; set; } = string.Empty;

    public string PrimaryColor { get; set; } = string.Empty;
    public string SecondaryColor { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}