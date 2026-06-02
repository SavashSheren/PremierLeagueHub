namespace PremierLeagueHub.WebUI.Models
{
    public class StandingsViewModel
    {
        public List<StandingRowViewModel> Rows { get; set; } = new List<StandingRowViewModel>();

        public StandingRowViewModel? Leader => Rows.FirstOrDefault();

        public StandingRowViewModel? BestAttack =>
            Rows.OrderByDescending(x => x.GoalsFor).FirstOrDefault();

        public StandingRowViewModel? BestDefense =>
            Rows.OrderBy(x => x.GoalsAgainst).FirstOrDefault();

        public int TotalTeams => Rows.Count;
    }
}