using PremierLeagueHub.DtoLayer.MatchEventDtos;

namespace PremierLeagueHub.WebUI.Models;

public class AdminMatchEventViewModel
{
    public int FixtureId { get; set; }

    public string HomeTeamName { get; set; } = string.Empty;
    public string HomeTeamLogoUrl { get; set; } = string.Empty;
    public int HomeTeamId { get; set; }

    public string AwayTeamName { get; set; } = string.Empty;
    public string AwayTeamLogoUrl { get; set; } = string.Empty;
    public int AwayTeamId { get; set; }

    public DateTime MatchDate { get; set; }
    public string StadiumName { get; set; } = string.Empty;

    public int TeamId { get; set; }
    public int Minute { get; set; }
    public string EventType { get; set; } = "Goal";
    public string PlayerName { get; set; } = string.Empty;
    public string AssistPlayerName { get; set; } = string.Empty;
    public string RelatedPlayerName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<ResultMatchEventDto> MatchEvents { get; set; } = new List<ResultMatchEventDto>();
}