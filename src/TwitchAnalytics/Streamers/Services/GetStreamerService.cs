using TwitchAnalytics.Streamers.Managers;
using TwitchAnalytics.Streamers.Models;

namespace TwitchAnalytics.Streamers.Services
{
    public class GetStreamerService
    {
        private readonly IStreamerManager streamerManager;

        public GetStreamerService(IStreamerManager streamerManager)
        {
            this.streamerManager = streamerManager;
        }

        public async Task<Streamer> GetStreamer(string streamerId)
        {
            if (string.IsNullOrWhiteSpace(streamerId) || streamerId.Equals("iker"))
            {
                throw new ArgumentException("Streamer ID cannot be empty", nameof(streamerId));
            }

            return await this.streamerManager.GetStreamer(streamerId);
        }
    }
}
