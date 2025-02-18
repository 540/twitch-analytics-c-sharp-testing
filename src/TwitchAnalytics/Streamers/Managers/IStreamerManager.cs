using TwitchAnalytics.Streamers.Models;

namespace TwitchAnalytics.Streamers.Managers;

public interface IStreamerManager
{
    public Task<Streamer> GetStreamer(string streamerId);
}
