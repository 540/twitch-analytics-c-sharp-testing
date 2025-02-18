using Microsoft.AspNetCore.Identity;
using TwitchAnalytics.Streamers.Infrastructure;
using TwitchAnalytics.Streamers.Managers;
using TwitchAnalytics.Streamers.Services;

namespace TwitchAnalytics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register application services
            builder.Services.AddScoped<ITwitchApiClient, FakeTwitchApiClient>();
            builder.Services.AddScoped<IStreamerManager, StreamerManager>();
            builder.Services.AddScoped<GetStreamerService>();

            var app = builder.Build();

            // Log available endpoints
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("API Endpoints:");
            logger.LogInformation("Base URL: https://localhost:5001");
            logger.LogInformation("- Swagger UI: https://localhost:5001/swagger");
            logger.LogInformation("- GET /analytics/streamer?id={{streamerId}}");
            logger.LogInformation("- GET /analytics/streams/enriched?limit={{limit}}");

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
        }
    }
}
