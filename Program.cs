using Microsoft.AspNetCore.Identity;
using TwitchAnalytics.Streamers.Infrastructure;
using TwitchAnalytics.Streamers.Managers;
using TwitchAnalytics.Streamers.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ITwitchApiClient, FakeTwitchApiClient>();
builder.Services.AddScoped<StreamerManager>();
builder.Services.AddScoped<GetStreamerService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
