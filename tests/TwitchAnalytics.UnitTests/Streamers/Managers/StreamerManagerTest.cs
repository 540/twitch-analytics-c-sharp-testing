using Moq;
using TwitchAnalytics.Streamers.Infrastructure;
using TwitchAnalytics.Streamers.Managers;
using TwitchAnalytics.Streamers.Models;
using Xunit;

namespace TwitchAnalytics.UnitTests.Streamers.Managers;

public class StreamerManagerTest
{
    [Fact]
    public async void DoesNotFindAnyStreamerForGivenId()
    {
        Mock<ITwitchApiClient> twitchApiClient = new Mock<ITwitchApiClient>();
        var streamerManager = new StreamerManager(twitchApiClient.Object);
        var response = new TwitchResponse();

        twitchApiClient.Setup(tac => tac.GetUserByIdAsync("1234")).ReturnsAsync(response);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => streamerManager.GetStreamer("1234"));
        Assert.Equal("Streamer with ID 1234 not found", exception.Message);
    }

    [Fact]
    public async void FindsStreamerForGivenId()
    {
        Mock<ITwitchApiClient> twitchApiClient = new Mock<ITwitchApiClient>();
        var streamerManager = new StreamerManager(twitchApiClient.Object);
        var response = new TwitchResponse();
        var streamer = new Streamer(
            "123",
            "name",
            "display_name",
            "description",
            "profile_image_url",
            "offline_image_url",
            "view_count",
            "follower_count",
            9,
            new DateTime()
        );
        response.Data.Add(streamer);

        twitchApiClient.Setup(tac => tac.GetUserByIdAsync("123")).ReturnsAsync(response);

        var foundStreamer = await streamerManager.GetStreamer("123");
        Assert.Equal(streamer, foundStreamer);
    }
}
