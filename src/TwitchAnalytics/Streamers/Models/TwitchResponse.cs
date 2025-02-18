using System.Collections.Generic;

namespace TwitchAnalytics.Streamers.Models
{
    public class TwitchResponse
    {
        public IEnumerable<Streamer> Data { get; set; } = new List<Streamer>();
    }
}
