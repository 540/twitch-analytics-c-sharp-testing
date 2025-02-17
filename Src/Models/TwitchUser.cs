namespace TwitchAnalytics.Models
{
    public class TwitchUser
    {
        public TwitchUser(
            string id,
            string login,
            string displayName,
            string type,
            string broadcasterType,
            string description,
            string profileImageUrl,
            string offlineImageUrl,
            int viewCount,
            DateTime createdAt)
        {
            Id = id;
            Login = login;
            DisplayName = displayName;
            Type = type;
            BroadcasterType = broadcasterType;
            Description = description;
            ProfileImageUrl = profileImageUrl;
            OfflineImageUrl = offlineImageUrl;
            ViewCount = viewCount;
            CreatedAt = createdAt;
        }

        public string Id { get; private set; }

        public string Login { get; private set; }

        public string DisplayName { get; private set; }

        public string Type { get; private set; }

        public string BroadcasterType { get; private set; }

        public string Description { get; private set; }

        public string ProfileImageUrl { get; private set; }

        public string OfflineImageUrl { get; private set; }

        public int ViewCount { get; private set; }

        public DateTime CreatedAt { get; private set; }
    }
}
