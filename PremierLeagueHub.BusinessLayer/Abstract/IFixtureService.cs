using PremierLeagueHub.DtoLayer.FixtureDtos;

namespace PremierLeagueHub.BusinessLayer.Abstract;

public interface IFixtureService
{
    Task<List<ResultFixtureDto>> GetAllFixturesAsync();
    Task<GetFixtureByIdDto?> GetFixtureByIdAsync(int id);
    Task CreateFixtureAsync(CreateFixtureDto createFixtureDto);
    Task<bool> UpdateFixtureAsync(UpdateFixtureDto updateFixtureDto);
    Task<bool> UpdateFixtureResultAsync(UpdateFixtureResultDto updateFixtureResultDto);
    Task<bool> DeleteFixtureAsync(int id);
}