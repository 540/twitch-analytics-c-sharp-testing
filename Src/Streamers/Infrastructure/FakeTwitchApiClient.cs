using System.Text.Json;
using Microsoft.Extensions.Logging;
using TwitchAnalytics.Streamers.Models;

namespace TwitchAnalytics.Streamers.Infrastructure
{
    public class FakeTwitchApiClient : ITwitchApiClient
    {
        private readonly TwitchResponse _fakeTwitchResponseData;
        private readonly ILogger<FakeTwitchApiClient> _logger;

        public FakeTwitchApiClient(ILogger<FakeTwitchApiClient> logger)
        {
            _logger = logger;
            try
            {
                _logger.LogInformation("Loading mock data from file");
                var json = File.ReadAllText("Data/twitch-mock-data.json");
                _logger.LogInformation("Mock data content: {Json}", json);

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                _fakeTwitchResponseData = JsonSerializer.Deserialize<TwitchResponse>(json, options)
                    ?? throw new InvalidOperationException("Failed to load mock data");

                _logger.LogInformation(
                    "Successfully loaded mock data with {Count} streamers",
                    _fakeTwitchResponseData.Data.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load mock data");
                throw;
            }
        }

        public Task<TwitchResponse> GetUserByIdAsync(string userId)
        {
            return Task.FromResult(_fakeTwitchResponseData);
        }
    }
}
