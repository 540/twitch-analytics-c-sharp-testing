using Microsoft.Extensions.Logging;
using TwitchAnalytics.Streamers.Infrastructure;
using TwitchAnalytics.Streamers.Models;

namespace TwitchAnalytics.Streamers.Managers
{
    public class StreamerManager
    {
        private readonly ITwitchApiClient _twitchClient;
        private readonly ILogger<StreamerManager> _logger;

        public StreamerManager(ITwitchApiClient twitchClient, ILogger<StreamerManager> logger)
        {
            _twitchClient = twitchClient;
            _logger = logger;
        }

        public async Task<Streamer> GetStreamer(string streamerId)
        {
            _logger.LogInformation("Getting streamer with ID: {StreamerId}", streamerId);
            TwitchResponse twitchResponse = await _twitchClient.GetUserByIdAsync(streamerId);
            _logger.LogInformation("Got response with {Count} streamers", twitchResponse.Data.Count());

            var streamer = twitchResponse.Data.FirstOrDefault();
            if (streamer == null)
            {
                _logger.LogWarning("No streamer found with ID: {StreamerId}", streamerId);
                throw new KeyNotFoundException($"Streamer with ID {streamerId} not found");
            }

            _logger.LogInformation("Found streamer: {StreamerName}", streamer.DisplayName);
            return streamer;
        }
    }
}
