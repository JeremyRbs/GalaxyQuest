using GalaxyQuest.Entities;
using GalaxyQuest.Services;
using Microsoft.AspNetCore.Authentication;
using Shard.Shared.Core;
using SystemClock = Shard.Shared.Core.SystemClock;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MapFactory>();
builder.Services.AddSingleton<MapRepository>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<IClock>(new SystemClock());
builder.Services.AddControllers();
builder.Services.AddHostedService<CombatUnitService>();
builder.Services.Configure<Dictionary<string, Wormholes>>(builder.Configuration.GetSection("Wormholes"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication("SchemeName")
    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandlerService>("SchemeName", options => {});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace Shard.GalaxyQuest
{
    public partial class Program { }
}