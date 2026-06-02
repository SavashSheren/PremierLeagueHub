using Microsoft.EntityFrameworkCore;
using PremierLeagueHub.BusinessLayer.Abstract;
using PremierLeagueHub.BusinessLayer.Concrete;
using PremierLeagueHub.BusinessLayer.Mapping;
using PremierLeagueHub.DataAccessLayer.Abstract;
using PremierLeagueHub.DataAccessLayer.Concrete;
using PremierLeagueHub.DataAccessLayer.EntityFramework;
using FluentValidation;
using PremierLeagueHub.BusinessLayer.ValidationRules.TeamValidators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PremierLeagueHubContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(typeof(GeneralMapping).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<CreateTeamDtoValidator>();

builder.Services.AddScoped<ITeamDal, EfTeamDal>();
builder.Services.AddScoped<ITeamService, TeamManager>();

builder.Services.AddScoped<IFixtureDal, EfFixtureDal>();
builder.Services.AddScoped<IFixtureService, FixtureManager>();

builder.Services.AddScoped<IMatchEventDal, EfMatchEventDal>();
builder.Services.AddScoped<IMatchEventService, MatchEventManager>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();