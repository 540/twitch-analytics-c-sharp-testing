using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TwitchAnalytics.Streamers.Infrastructure;

namespace TwitchAnalytics.Streams.Controllers
{
    [ApiController]
    [Route("analytics/streams")]
    public class EnrichedStreamsController : ControllerBase
    {
        public EnrichedStreamsController()
        {
        }

        [HttpGet("enriched")]
        [ProducesResponseType(typeof(List<EnrichedStream>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEnrichedStreams(int limit = 3)
        {
            try
            {
                if (limit <= 0 || limit > 100)
                {
                    return this.BadRequest(
                        new ErrorResponse { Error = "Invalid 'limit' parameter. Must be between 1 and 100." });
                }

                // 1. Get top streams
                var streamsJson = await System.IO.File.ReadAllTextAsync("Data/twitch-streams-mock.json");
                var usersJson = await System.IO.File.ReadAllTextAsync("Data/twitch-users-mock.json");

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var streamsResponse = JsonSerializer.Deserialize<JsonDocument>(streamsJson, options);
                var usersResponse = JsonSerializer.Deserialize<JsonDocument>(usersJson, options);

                if (streamsResponse == null || usersResponse == null)
                {
                    throw new InvalidOperationException("Failed to load mock data");
                }

                // 2. Get top streams ordered by viewer count
                var topStreams = streamsResponse.RootElement
                    .GetProperty("data")
                    .EnumerateArray()
                    .Select(
                        stream => new
                        {
                            stream_id = stream.GetProperty("id").GetString(),
                            user_id = stream.GetProperty("user_id").GetString(),
                            title = stream.GetProperty("title").GetString(),
                            viewer_count = stream.GetProperty("viewer_count").GetInt32(),
                            game_name = stream.GetProperty("game_name").GetString(),
                            started_at = stream.GetProperty("started_at").GetString()
                        })
                    .OrderByDescending(s => s.viewer_count)
                    .Take(limit)
                    .ToList();

                // 3. Get user details for those streams
                var userIds = topStreams.Select(s => s.user_id).ToList();
                var users = usersResponse.RootElement
                    .GetProperty("data")
                    .EnumerateArray()
                    .Where(user => userIds.Contains(user.GetProperty("id").GetString()))
                    .ToDictionary(
                        user => user.GetProperty("id").GetString()!,
                        user => new
                        {
                            display_name = user.GetProperty("display_name").GetString(),
                            profile_image_url = user.GetProperty("profile_image_url").GetString(),
                            broadcaster_type = user.GetProperty("broadcaster_type").GetString()
                        });

                // 4. Combine stream and user data
                var enrichedStreams = topStreams.Select(
                    stream =>
                    {
                        var user = users[stream.user_id!];
                        return new
                        {
                            stream_id = stream.stream_id,
                            user_id = stream.user_id,
                            title = stream.title,
                            viewer_count = stream.viewer_count,
                            game_name = stream.game_name,
                            started_at = stream.started_at,
                            user_display_name = user.display_name,
                            profile_image_url = user.profile_image_url,
                            broadcaster_type = user.broadcaster_type
                        };
                    }).ToList();

                return this.Ok(enrichedStreams);
            }
            catch (Exception)
            {
                return this.StatusCode(
                    500,
                    new ErrorResponse { Error = "An unexpected error occurred while processing your request." });
            }
        }

        private class EnrichedStream
        {
            public string StreamId { get; set; } = string.Empty;

            public string UserId { get; set; } = string.Empty;

            public string UserName { get; set; } = string.Empty;

            public int ViewerCount { get; set; }

            public string Title { get; set; } = string.Empty;

            public string UserDisplayName { get; set; } = string.Empty;

            public string ProfileImageUrl { get; set; } = string.Empty;
        }
    }
}
