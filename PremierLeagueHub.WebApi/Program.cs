using Microsoft.EntityFrameworkCore;
using PremierLeagueHub.BusinessLayer.Abstract;
using PremierLeagueHub.BusinessLayer.Concrete;
using PremierLeagueHub.BusinessLayer.Mapping;
using PremierLeagueHub.DataAccessLayer.Abstract;
using PremierLeagueHub.DataAccessLayer.Concrete;

using PremierLeagueHub.DataAccessLayer.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PremierLeagueHubContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(typeof(GeneralMapping));

builder.Services.AddScoped<ITeamDal, EfTeamDal>();
builder.Services.AddScoped<ITeamService, TeamManager>();

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