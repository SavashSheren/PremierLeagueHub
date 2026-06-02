namespace PremierLeagueHub.EntityLayer.Entities;

public class MatchEvent
{
    public int MatchEventId { get; set; }

    public int FixtureId { get; set; }
    public Fixture? Fixture { get; set; }

    public int TeamId { get; set; }
    public Team? Team { get; set; }

    public int Minute { get; set; }

    public string EventType { get; set; } = "Goal";

    public string PlayerName { get; set; } = string.Empty;

    public string AssistPlayerName { get; set; } = string.Empty;

    public string RelatedPlayerName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}