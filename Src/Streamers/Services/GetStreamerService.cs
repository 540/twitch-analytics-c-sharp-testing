using TwitchAnalytics.Streamers.Managers;
using TwitchAnalytics.Streamers.Models;

namespace TwitchAnalytics.Streamers.Services
{
    public class GetStreamerService
    {
        private readonly StreamerManager _streamerManager;

        public GetStreamerService(StreamerManager streamerManager)
        {
            _streamerManager = streamerManager;
        }

        public async Task<Streamer> GetStreamer(string streamerId)
        {
            if (string.IsNullOrWhiteSpace(streamerId))
            {
                throw new ArgumentException("Streamer ID cannot be empty", nameof(streamerId));
            }

            return await _streamerManager.GetStreamer(streamerId);
        }
    }
}
