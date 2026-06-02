namespace PremierLeagueHub.WebUI.Models;

public class StandingsViewModel
{
    public List<StandingRowViewModel> Rows { get; set; } = new List<StandingRowViewModel>();

    public int FinishedFixturesCount { get; set; }
    public int ScheduledFixturesCount { get; set; }

    public StandingRowViewModel? Leader => Rows.FirstOrDefault();

    public StandingRowViewModel? BestAttack =>
        Rows.OrderByDescending(x => x.GoalsFor).FirstOrDefault();

    public StandingRowViewModel? BestDefense =>
        Rows
            .Where(x => x.Played > 0)
            .OrderBy(x => x.GoalsAgainst)
            .FirstOrDefault();

    public int TotalTeams => Rows.Count;
}