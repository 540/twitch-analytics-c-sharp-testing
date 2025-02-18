using TwitchAnalytics.Streamers.Infrastructure;
using TwitchAnalytics.Streamers.Models;

namespace TwitchAnalytics.Streamers.Managers
{
    public class StreamerManager : IStreamerManager
    {
        private readonly ITwitchApiClient twitchClient;

        public StreamerManager(ITwitchApiClient twitchClient)
        {
            this.twitchClient = twitchClient;
        }

        public async Task<Streamer> GetStreamer(string streamerId)
        {
            TwitchResponse twitchResponse = await this.twitchClient.GetUserByIdAsync(streamerId);

            Streamer? streamer = twitchResponse.Data.FirstOrDefault();
            if (streamer == null)
            {
                throw new KeyNotFoundException($"Streamer with ID {streamerId} not found");
            }

            return streamer;
        }
    }
}
