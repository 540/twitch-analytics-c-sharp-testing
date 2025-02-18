using TwitchAnalytics.Streamers.Managers;
using TwitchAnalytics.Streamers.Models;

namespace TwitchAnalytics.Streamers.Services
{
    public class GetStreamerService
    {
        private readonly StreamerManager streamerManager;

        public GetStreamerService(StreamerManager streamerManager)
        {
            this.streamerManager = streamerManager;
        }

        public async Task<Streamer> GetStreamer(string streamerId)
        {
            if (string.IsNullOrWhiteSpace(streamerId))
            {
                throw new ArgumentException("Streamer ID cannot be empty", nameof(streamerId));
            }

            return await this.streamerManager.GetStreamer(streamerId);
        }
    }
}
