using TwitchAnalytics.Models;

namespace TwitchAnalytics.Services
{
    public class FakeTwitchService : ITwitchService
    {
        private readonly Dictionary<string, TwitchUser> _fakeUsers;

        public FakeTwitchService()
        {
            _fakeUsers = new Dictionary<string, TwitchUser>
            {
                {
                    "12345", new TwitchUser(
                        "12345",
                        "ninja",
                        "Ninja",
                        string.Empty,
                        "partner",
                        "Professional Gamer and Streamer",
                        "https://example.com/ninja.jpg",
                        "https://example.com/ninja-offline.jpg",
                        500000,
                        DateTime.Parse("2011-11-20T00:00:00Z"))
                }
            };
        }

        public async Task<TwitchUser> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be empty", nameof(userId));
            }

            await Task.Delay(100);

            if (_fakeUsers.TryGetValue(userId, out var user))
            {
                return user;
            }

            throw new KeyNotFoundException($"User with ID {userId} not found");
        }
    }
}
