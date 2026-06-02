using AutoMapper;
using PremierLeagueHub.BusinessLayer.Abstract;
using PremierLeagueHub.DataAccessLayer.Abstract;
using PremierLeagueHub.DtoLayer.FixtureDtos;
using PremierLeagueHub.EntityLayer.Entities;

namespace PremierLeagueHub.BusinessLayer.Concrete;

public class FixtureManager : IFixtureService
{
    private readonly IFixtureDal _fixtureDal;
    private readonly IMapper _mapper;

    public FixtureManager(IFixtureDal fixtureDal, IMapper mapper)
    {
        _fixtureDal = fixtureDal;
        _mapper = mapper;
    }

    public async Task<List<ResultFixtureDto>> GetAllFixturesAsync()
    {
        var fixtures = await _fixtureDal.GetAllFixturesWithTeamsAsync();

        return fixtures.Select(MapToResultFixtureDto).ToList();
    }

    public async Task<GetFixtureByIdDto?> GetFixtureByIdAsync(int id)
    {
        var fixture = await _fixtureDal.GetFixtureWithTeamsByIdAsync(id);

        if (fixture == null)
        {
            return null;
        }

        return MapToGetFixtureByIdDto(fixture);
    }

    public async Task CreateFixtureAsync(CreateFixtureDto createFixtureDto)
    {
        var fixture = _mapper.Map<Fixture>(createFixtureDto);
        await _fixtureDal.InsertAsync(fixture);
    }

    public async Task<bool> UpdateFixtureAsync(UpdateFixtureDto updateFixtureDto)
    {
        var existingFixture = await _fixtureDal.GetByIdAsync(updateFixtureDto.FixtureId);

        if (existingFixture == null)
        {
            return false;
        }

        _mapper.Map(updateFixtureDto, existingFixture);

        await _fixtureDal.UpdateAsync(existingFixture);
        return true;
    }

    public async Task<bool> UpdateFixtureResultAsync(UpdateFixtureResultDto updateFixtureResultDto)
    {
        var fixture = await _fixtureDal.GetByIdAsync(updateFixtureResultDto.FixtureId);

        if (fixture == null)
        {
            return false;
        }

        fixture.HomeScore = updateFixtureResultDto.HomeScore;
        fixture.AwayScore = updateFixtureResultDto.AwayScore;
        fixture.Status = string.IsNullOrWhiteSpace(updateFixtureResultDto.Status)
            ? "Finished"
            : updateFixtureResultDto.Status;

        await _fixtureDal.UpdateAsync(fixture);
        return true;
    }

    public async Task<bool> DeleteFixtureAsync(int id)
    {
        var fixture = await _fixtureDal.GetByIdAsync(id);

        if (fixture == null)
        {
            return false;
        }

        await _fixtureDal.DeleteAsync(fixture);
        return true;
    }

    private static ResultFixtureDto MapToResultFixtureDto(Fixture fixture)
    {
        return new ResultFixtureDto
        {
            FixtureId = fixture.FixtureId,

            HomeTeamId = fixture.HomeTeamId,
            HomeTeamName = fixture.HomeTeam?.TeamName ?? string.Empty,
            HomeTeamShortName = fixture.HomeTeam?.ShortName ?? string.Empty,
            HomeTeamLogoUrl = fixture.HomeTeam?.LogoUrl ?? string.Empty,

            AwayTeamId = fixture.AwayTeamId,
            AwayTeamName = fixture.AwayTeam?.TeamName ?? string.Empty,
            AwayTeamShortName = fixture.AwayTeam?.ShortName ?? string.Empty,
            AwayTeamLogoUrl = fixture.AwayTeam?.LogoUrl ?? string.Empty,

            MatchDate = fixture.MatchDate,
            WeekNumber = fixture.WeekNumber,
            Season = fixture.Season,
            StadiumName = fixture.StadiumName,
            IsTopMatch = fixture.IsTopMatch,

            HomeScore = fixture.HomeScore,
            AwayScore = fixture.AwayScore,

            Status = fixture.Status
        };
    }

    private static GetFixtureByIdDto MapToGetFixtureByIdDto(Fixture fixture)
    {
        return new GetFixtureByIdDto
        {
            FixtureId = fixture.FixtureId,

            HomeTeamId = fixture.HomeTeamId,
            HomeTeamName = fixture.HomeTeam?.TeamName ?? string.Empty,
            HomeTeamShortName = fixture.HomeTeam?.ShortName ?? string.Empty,
            HomeTeamLogoUrl = fixture.HomeTeam?.LogoUrl ?? string.Empty,

            AwayTeamId = fixture.AwayTeamId,
            AwayTeamName = fixture.AwayTeam?.TeamName ?? string.Empty,
            AwayTeamShortName = fixture.AwayTeam?.ShortName ?? string.Empty,
            AwayTeamLogoUrl = fixture.AwayTeam?.LogoUrl ?? string.Empty,

            MatchDate = fixture.MatchDate,
            WeekNumber = fixture.WeekNumber,
            Season = fixture.Season,
            StadiumName = fixture.StadiumName,
            IsTopMatch = fixture.IsTopMatch,

            HomeScore = fixture.HomeScore,
            AwayScore = fixture.AwayScore,

            Status = fixture.Status
        };
    }
}