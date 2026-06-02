namespace PremierLeagueHub.DtoLayer.MatchEventDtos;

public class CreateMatchEventDto
{
    public int FixtureId { get; set; }

    public int TeamId { get; set; }

    public int Minute { get; set; }

    public string EventType { get; set; } = "Goal";

    public string PlayerName { get; set; } = string.Empty;

    public string AssistPlayerName { get; set; } = string.Empty;

    public string RelatedPlayerName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}