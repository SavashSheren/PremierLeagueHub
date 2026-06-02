namespace PremierLeagueHub.DtoLayer.FixtureDtos;

public class UpdateFixtureResultDto
{
    public int FixtureId { get; set; }

    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    public string Status { get; set; } = "Finished";
}