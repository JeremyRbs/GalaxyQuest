using GalaxyQuest.Services;
using Shard.Shared.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MapFactory>();
builder.Services.AddSingleton<MapRepository>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<IClock>(new SystemClock());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace Shard.GalaxyQuest
{
    public partial class Program { }
}