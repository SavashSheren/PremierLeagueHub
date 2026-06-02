using AutoMapper;
using PremierLeagueHub.DtoLayer.FixtureDtos;
using PremierLeagueHub.DtoLayer.MatchEventDtos;
using PremierLeagueHub.DtoLayer.TeamDtos;
using PremierLeagueHub.EntityLayer.Entities;

namespace PremierLeagueHub.BusinessLayer.Mapping;

public class GeneralMapping : Profile
{
    public GeneralMapping()
    {
        CreateMap<Team, ResultTeamDto>().ReverseMap();
        CreateMap<Team, GetTeamByIdDto>().ReverseMap();
        CreateMap<Team, CreateTeamDto>().ReverseMap();
        CreateMap<Team, UpdateTeamDto>().ReverseMap();

        CreateMap<Fixture, CreateFixtureDto>().ReverseMap();
        CreateMap<Fixture, UpdateFixtureDto>().ReverseMap();

        CreateMap<MatchEvent, CreateMatchEventDto>().ReverseMap();
        CreateMap<MatchEvent, UpdateMatchEventDto>().ReverseMap();
    }
}