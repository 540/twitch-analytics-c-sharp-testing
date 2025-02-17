using TwitchAnalytics.Models;

namespace TwitchAnalytics.Services
{
    public interface ITwitchService
    {
        Task<TwitchUser> GetUserByIdAsync(string userId);
    }
}
