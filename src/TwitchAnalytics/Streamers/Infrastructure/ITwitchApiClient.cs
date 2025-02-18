using TwitchAnalytics.Streamers.Models;

namespace TwitchAnalytics.Streamers.Infrastructure
{
    public interface ITwitchApiClient
    {
        Task<TwitchResponse> GetUserByIdAsync(string userId);
    }
}
