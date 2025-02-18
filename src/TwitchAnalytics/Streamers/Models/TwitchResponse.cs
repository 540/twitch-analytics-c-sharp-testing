using System.Collections.Generic;

namespace TwitchAnalytics.Streamers.Models
{
    public class TwitchResponse
    {
        public List<Streamer> Data { get; set; } = new List<Streamer>();
    }
}
