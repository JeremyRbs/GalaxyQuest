using GalaxyQuest.Services;
<<<<<<< HEAD
=======
using Shard.Shared.Core;
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MapFactory>();
builder.Services.AddSingleton<MapRepository>();
builder.Services.AddSingleton<UserRepository>();
<<<<<<< HEAD
=======
builder.Services.AddSingleton<IClock>(new SystemClock());
>>>>>>> 90999a61d78ffa28d5e8dfebe0c283f46b4cdf0a
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