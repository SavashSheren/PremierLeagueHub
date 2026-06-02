using Microsoft.AspNetCore.Mvc;
using PremierLeagueHub.BusinessLayer.Abstract;
using PremierLeagueHub.DtoLayer.FixtureDtos;
using PremierLeagueHub.DtoLayer.TeamDtos;

namespace PremierLeagueHub.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevelopmentSeedController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IFixtureService _fixtureService;
    private readonly IWebHostEnvironment _environment;

    public DevelopmentSeedController(
        ITeamService teamService,
        IFixtureService fixtureService,
        IWebHostEnvironment environment)
    {
        _teamService = teamService;
        _fixtureService = fixtureService;
        _environment = environment;
    }

    [HttpPost("premier-league-teams")]
    public async Task<IActionResult> SeedPremierLeagueTeams()
    {
        if (!_environment.IsDevelopment())
        {
            return BadRequest("Seed endpoint is only available in Development environment.");
        }

        var existingTeams = await _teamService.GetAllTeamsAsync();
        var teams = GetPremierLeagueTeams();

        var createdCount = 0;
        var skippedCount = 0;

        foreach (var team in teams)
        {
            var alreadyExists = existingTeams.Any(x =>
                x.TeamName.Equals(team.TeamName, StringComparison.OrdinalIgnoreCase));

            if (alreadyExists)
            {
                skippedCount++;
                continue;
            }

            await _teamService.CreateTeamAsync(team);
            createdCount++;
        }

        return Ok(new
        {
            Message = "Premier League seed operation completed.",
            CreatedCount = createdCount,
            SkippedCount = skippedCount,
            TotalSeedTeams = teams.Count
        });
    }

    [HttpPost("week-34-fixtures")]
    public async Task<IActionResult> SeedWeek34Fixtures()
    {
        if (!_environment.IsDevelopment())
        {
            return BadRequest("Seed endpoint is only available in Development environment.");
        }

        var teams = await _teamService.GetAllTeamsAsync();
        var existingFixtures = await _fixtureService.GetAllFixturesAsync();
        var fixtureSeeds = GetWeek34FixtureSeeds();

        var missingTeams = fixtureSeeds
            .SelectMany(x => new[] { x.HomeShortName, x.AwayShortName })
            .Distinct()
            .Where(shortName => !teams.Any(team =>
                team.ShortName.Equals(shortName, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (missingTeams.Any())
        {
            return BadRequest(new
            {
                Message = "Some teams are missing. Run premier-league-teams seed first.",
                MissingTeams = missingTeams
            });
        }

        int GetTeamId(string shortName)
        {
            return teams.First(x =>
                x.ShortName.Equals(shortName, StringComparison.OrdinalIgnoreCase)).TeamId;
        }

        var createdCount = 0;
        var skippedCount = 0;

        foreach (var seed in fixtureSeeds)
        {
            var homeTeamId = GetTeamId(seed.HomeShortName);
            var awayTeamId = GetTeamId(seed.AwayShortName);

            var alreadyExists = existingFixtures.Any(x =>
                x.HomeTeamId == homeTeamId &&
                x.AwayTeamId == awayTeamId &&
                x.MatchDate == seed.MatchDate);

            if (alreadyExists)
            {
                skippedCount++;
                continue;
            }

            var fixture = new CreateFixtureDto
            {
                HomeTeamId = homeTeamId,
                AwayTeamId = awayTeamId,
                MatchDate = seed.MatchDate,
                WeekNumber = seed.WeekNumber,
                Season = seed.Season,
                StadiumName = seed.StadiumName,
                IsTopMatch = seed.IsTopMatch,
                Status = seed.Status
            };

            await _fixtureService.CreateFixtureAsync(fixture);
            createdCount++;
        }

        return Ok(new
        {
            Message = "Week 34 fixture seed operation completed.",
            CreatedCount = createdCount,
            SkippedCount = skippedCount,
            TotalSeedFixtures = fixtureSeeds.Count
        });
    }

    private static List<CreateTeamDto> GetPremierLeagueTeams()
    {
        return new List<CreateTeamDto>
        {
            new()
            {
                TeamName = "Arsenal",
                ShortName = "ARS",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/5/53/Arsenal_FC.svg",
                City = "London",
                StadiumName = "Emirates Stadium",
                FoundedYear = 1886,
                ManagerName = "Mikel Arteta",
                WebsiteUrl = "https://www.arsenal.com",
                Description = "Arsenal Football Club is one of the most successful clubs in English football.",
                PrimaryColor = "#EF0107",
                SecondaryColor = "#FFFFFF",
                IsActive = true
            },
            new()
            {
                TeamName = "Aston Villa",
                ShortName = "AVL",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/9/9a/Aston_Villa_FC_new_crest.svg",
                City = "Birmingham",
                StadiumName = "Villa Park",
                FoundedYear = 1874,
                ManagerName = "Unai Emery",
                WebsiteUrl = "https://www.avfc.co.uk",
                Description = "Aston Villa is a historic English football club based in Birmingham.",
                PrimaryColor = "#670E36",
                SecondaryColor = "#95BFE5",
                IsActive = true
            },
            new()
            {
                TeamName = "AFC Bournemouth",
                ShortName = "BOU",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/e/e5/AFC_Bournemouth_%282013%29.svg",
                City = "Bournemouth",
                StadiumName = "Vitality Stadium",
                FoundedYear = 1899,
                ManagerName = "Andoni Iraola",
                WebsiteUrl = "https://www.afcb.co.uk",
                Description = "AFC Bournemouth is a professional football club based on the south coast of England.",
                PrimaryColor = "#DA291C",
                SecondaryColor = "#000000",
                IsActive = true
            },
            new()
            {
                TeamName = "Brentford",
                ShortName = "BRE",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/2/2a/Brentford_FC_crest.svg",
                City = "London",
                StadiumName = "Gtech Community Stadium",
                FoundedYear = 1889,
                ManagerName = "Keith Andrews",
                WebsiteUrl = "https://www.brentfordfc.com",
                Description = "Brentford Football Club is a London-based club known for modern football operations.",
                PrimaryColor = "#E30613",
                SecondaryColor = "#FFFFFF",
                IsActive = true
            },
            new()
            {
                TeamName = "Brighton & Hove Albion",
                ShortName = "BHA",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/f/fd/Brighton_%26_Hove_Albion_logo.svg",
                City = "Brighton",
                StadiumName = "American Express Stadium",
                FoundedYear = 1901,
                ManagerName = "Fabian Hurzeler",
                WebsiteUrl = "https://www.brightonandhovealbion.com",
                Description = "Brighton & Hove Albion is a modern Premier League club from the south coast.",
                PrimaryColor = "#0057B8",
                SecondaryColor = "#FFFFFF",
                IsActive = true
            },
            new()
            {
                TeamName = "Burnley",
                ShortName = "BUR",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/0/02/Burnley_FC_badge.png",
                City = "Burnley",
                StadiumName = "Turf Moor",
                FoundedYear = 1882,
                ManagerName = "Scott Parker",
                WebsiteUrl = "https://www.burnleyfootballclub.com",
                Description = "Burnley Football Club is one of England's traditional football clubs.",
                PrimaryColor = "#6C1D45",
                SecondaryColor = "#99D6EA",
                IsActive = true
            },
            new()
            {
                TeamName = "Chelsea",
                ShortName = "CHE",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/c/cc/Chelsea_FC.svg",
                City = "London",
                StadiumName = "Stamford Bridge",
                FoundedYear = 1905,
                ManagerName = "Liam Rosenior",
                WebsiteUrl = "https://www.chelseafc.com",
                Description = "Chelsea Football Club is a major London club with domestic and European success.",
                PrimaryColor = "#034694",
                SecondaryColor = "#FFFFFF",
                IsActive = true
            },
            new()
            {
                TeamName = "Crystal Palace",
                ShortName = "CRY",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/a/a2/Crystal_Palace_FC_logo_%282022%29.svg",
                City = "London",
                StadiumName = "Selhurst Park",
                FoundedYear = 1905,
                ManagerName = "Oliver Glasner",
                WebsiteUrl = "https://www.cpfc.co.uk",
                Description = "Crystal Palace Football Club is based in south London and plays at Selhurst Park.",
                PrimaryColor = "#1B458F",
                SecondaryColor = "#C4122E",
                IsActive = true
            },
            new()
            {
                TeamName = "Everton",
                ShortName = "EVE",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/7/7c/Everton_FC_logo.svg",
                City = "Liverpool",
                StadiumName = "Hill Dickinson Stadium",
                FoundedYear = 1878,
                ManagerName = "David Moyes",
                WebsiteUrl = "https://www.evertonfc.com",
                Description = "Everton Football Club is a historic club from Liverpool.",
                PrimaryColor = "#003399",
                SecondaryColor = "#FFFFFF",
                IsActive = true
            },
            new()
            {
                TeamName = "Fulham",
                ShortName = "FUL",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/e/eb/Fulham_FC_%28shield%29.svg",
                City = "London",
                StadiumName = "Craven Cottage",
                FoundedYear = 1879,
                ManagerName = "Marco Silva",
                WebsiteUrl = "https://www.fulhamfc.com",
                Description = "Fulham Football Club is a London club based at Craven Cottage.",
                PrimaryColor = "#000000",
                SecondaryColor = "#FFFFFF",
                IsActive = true
            },
            new()
            {
                TeamName = "Leeds United",
                ShortName = "LEE",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/5/54/Leeds_United_F.C._logo.svg",
                City = "Leeds",
                StadiumName = "Elland Road",
                FoundedYear = 1919,
                ManagerName = "Daniel Farke",
                WebsiteUrl = "https://www.leedsunited.com",
                Description = "Leeds United is a historic football club based in West Yorkshire.",
                PrimaryColor = "#FFCD00",
                SecondaryColor = "#1D428A",
                IsActive = true
            },
            new()
            {
                TeamName = "Liverpool",
                ShortName = "LIV",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/0/0c/Liverpool_FC.svg",
                City = "Liverpool",
                StadiumName = "Anfield",
                FoundedYear = 1892,
                ManagerName = "Arne Slot",
                WebsiteUrl = "https://www.liverpoolfc.com",
                Description = "Liverpool Football Club is one of the most decorated clubs in English football.",
                PrimaryColor = "#C8102E",
                SecondaryColor = "#FFFFFF",
                IsActive = true
            },
            new()
            {
                TeamName = "Manchester City",
                ShortName = "MCI",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/e/eb/Manchester_City_FC_badge.svg",
                City = "Manchester",
                StadiumName = "Etihad Stadium",
                FoundedYear = 1880,
                ManagerName = "Pep Guardiola",
                WebsiteUrl = "https://www.mancity.com",
                Description = "Manchester City is a leading Premier League club based in Manchester.",
                PrimaryColor = "#6CABDD",
                SecondaryColor = "#FFFFFF",
                IsActive = true
            },
            new()
            {
                TeamName = "Manchester United",
                ShortName = "MUN",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/7/7a/Manchester_United_FC_crest.svg",
                City = "Manchester",
                StadiumName = "Old Trafford",
                FoundedYear = 1878,
                ManagerName = "Michael Carrick",
                WebsiteUrl = "https://www.manutd.com",
                Description = "Manchester United is one of the most globally recognised football clubs.",
                PrimaryColor = "#DA291C",
                SecondaryColor = "#FBE122",
                IsActive = true
            },
            new()
            {
                TeamName = "Newcastle United",
                ShortName = "NEW",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/5/56/Newcastle_United_Logo.svg",
                City = "Newcastle upon Tyne",
                StadiumName = "St James' Park",
                FoundedYear = 1892,
                ManagerName = "Eddie Howe",
                WebsiteUrl = "https://www.newcastleunited.com",
                Description = "Newcastle United is a major club from the north east of England.",
                PrimaryColor = "#241F20",
                SecondaryColor = "#FFFFFF",
                IsActive = true
            },
            new()
            {
                TeamName = "Nottingham Forest",
                ShortName = "NFO",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/e/e5/Nottingham_Forest_F.C._logo.svg",
                City = "Nottingham",
                StadiumName = "City Ground",
                FoundedYear = 1865,
                ManagerName = "Vitor Pereira",
                WebsiteUrl = "https://www.nottinghamforest.co.uk",
                Description = "Nottingham Forest is a historic English club with European heritage.",
                PrimaryColor = "#DD0000",
                SecondaryColor = "#FFFFFF",
                IsActive = true
            },
            new()
            {
                TeamName = "Sunderland",
                ShortName = "SUN",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/7/77/Logo_Sunderland.svg",
                City = "Sunderland",
                StadiumName = "Stadium of Light",
                FoundedYear = 1879,
                ManagerName = "Regis Le Bris",
                WebsiteUrl = "https://www.safc.com",
                Description = "Sunderland AFC is a historic football club from the north east of England.",
                PrimaryColor = "#EB172B",
                SecondaryColor = "#FFFFFF",
                IsActive = true
            },
            new()
            {
                TeamName = "Tottenham Hotspur",
                ShortName = "TOT",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/b/b4/Tottenham_Hotspur.svg",
                City = "London",
                StadiumName = "Tottenham Hotspur Stadium",
                FoundedYear = 1882,
                ManagerName = "Roberto De Zerbi",
                WebsiteUrl = "https://www.tottenhamhotspur.com",
                Description = "Tottenham Hotspur is a north London club with a modern stadium and global fanbase.",
                PrimaryColor = "#132257",
                SecondaryColor = "#FFFFFF",
                IsActive = true
            },
            new()
            {
                TeamName = "West Ham United",
                ShortName = "WHU",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/c/c2/West_Ham_United_FC_logo.svg",
                City = "London",
                StadiumName = "London Stadium",
                FoundedYear = 1895,
                ManagerName = "Nuno Espirito Santo",
                WebsiteUrl = "https://www.whufc.com",
                Description = "West Ham United is an east London club playing at London Stadium.",
                PrimaryColor = "#7A263A",
                SecondaryColor = "#1BB1E7",
                IsActive = true
            },
            new()
            {
                TeamName = "Wolverhampton Wanderers",
                ShortName = "WOL",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/f/fc/Wolverhampton_Wanderers.svg",
                City = "Wolverhampton",
                StadiumName = "Molineux Stadium",
                FoundedYear = 1877,
                ManagerName = "Rob Edwards",
                WebsiteUrl = "https://www.wolves.co.uk",
                Description = "Wolverhampton Wanderers is a traditional English football club based in Wolverhampton.",
                PrimaryColor = "#FDB913",
                SecondaryColor = "#231F20",
                IsActive = true
            }
        };
    }

    private static List<FixtureSeed> GetWeek34FixtureSeeds()
    {
        return new List<FixtureSeed>
        {
            new("MCI", "CHE", new DateTime(2026, 4, 18, 20, 0, 0), 34, "2025/2026", "Etihad Stadium", true, "Scheduled"),
            new("ARS", "LIV", new DateTime(2026, 4, 19, 17, 30, 0), 34, "2025/2026", "Emirates Stadium", true, "Scheduled"),
            new("MUN", "TOT", new DateTime(2026, 4, 19, 20, 0, 0), 34, "2025/2026", "Old Trafford", true, "Scheduled"),
            new("NEW", "AVL", new DateTime(2026, 4, 20, 14, 30, 0), 34, "2025/2026", "St James' Park", false, "Scheduled"),
            new("WHU", "CRY", new DateTime(2026, 4, 20, 16, 0, 0), 34, "2025/2026", "London Stadium", false, "Scheduled"),
            new("FUL", "BRE", new DateTime(2026, 4, 20, 16, 0, 0), 34, "2025/2026", "Craven Cottage", false, "Scheduled"),
            new("BHA", "BOU", new DateTime(2026, 4, 20, 16, 0, 0), 34, "2025/2026", "American Express Stadium", false, "Scheduled"),
            new("EVE", "NFO", new DateTime(2026, 4, 20, 18, 30, 0), 34, "2025/2026", "Hill Dickinson Stadium", false, "Scheduled"),
            new("LEE", "SUN", new DateTime(2026, 4, 21, 20, 0, 0), 34, "2025/2026", "Elland Road", false, "Scheduled"),
            new("WOL", "BUR", new DateTime(2026, 4, 21, 22, 0, 0), 34, "2025/2026", "Molineux Stadium", false, "Scheduled")
        };
    }

    private sealed record FixtureSeed(
        string HomeShortName,
        string AwayShortName,
        DateTime MatchDate,
        int WeekNumber,
        string Season,
        string StadiumName,
        bool IsTopMatch,
        string Status);
}