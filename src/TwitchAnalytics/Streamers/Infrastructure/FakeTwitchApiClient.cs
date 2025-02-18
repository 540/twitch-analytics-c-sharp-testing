using System.Text.Json;
using Microsoft.Extensions.Logging;
using TwitchAnalytics.Streamers.Models;

namespace TwitchAnalytics.Streamers.Infrastructure
{
    public class FakeTwitchApiClient : ITwitchApiClient
    {
        private readonly TwitchResponse fakeTwitchResponseData;
        private readonly ILogger<FakeTwitchApiClient> logger;

        public FakeTwitchApiClient(ILogger<FakeTwitchApiClient> logger)
        {
            this.logger = logger;
            try
            {
                this.logger.LogInformation("Loading mock data from file");
                var json = File.ReadAllText("Data/twitch-mock-data.json");
                this.logger.LogInformation("Mock data content: {Json}", json);

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                this.fakeTwitchResponseData = JsonSerializer.Deserialize<TwitchResponse>(json, options)
                    ?? throw new InvalidOperationException("Failed to load mock data");

                this.logger.LogInformation(
                    "Successfully loaded mock data with {Count} streamers",
                    this.fakeTwitchResponseData.Data.Count());
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to load mock data");
                throw;
            }
        }

        public Task<TwitchResponse> GetUserByIdAsync(string userId)
        {
            return Task.FromResult(this.fakeTwitchResponseData);
        }
    }
}
