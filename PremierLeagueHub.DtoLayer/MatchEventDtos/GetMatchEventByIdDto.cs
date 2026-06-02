namespace PremierLeagueHub.DtoLayer.MatchEventDtos;

public class GetMatchEventByIdDto
{
    public int MatchEventId { get; set; }

    public int FixtureId { get; set; }

    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string TeamShortName { get; set; } = string.Empty;
    public string TeamLogoUrl { get; set; } = string.Empty;

    public int Minute { get; set; }

    public string EventType { get; set; } = string.Empty;

    public string PlayerName { get; set; } = string.Empty;

    public string AssistPlayerName { get; set; } = string.Empty;

    public string RelatedPlayerName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}