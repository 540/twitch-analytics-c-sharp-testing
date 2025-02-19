using Microsoft.AspNetCore.Mvc;
using Moq;
using TwitchAnalytics.Streamers.Controllers;
using TwitchAnalytics.Streamers.Infrastructure;
using TwitchAnalytics.Streamers.Managers;
using TwitchAnalytics.Streamers.Models;
using TwitchAnalytics.Streamers.Services;
using Xunit;

namespace TwitchAnalytics.UnitTests.Streamers.Controllers;

public class StreamerControllerTest
{
    [Fact]
    public async Task ErrorIfStreamerIdNotGiven()
    {
        var streamerManager = new Mock<IStreamerManager>();
        var getStreamerService = new GetStreamerService(streamerManager.Object);
        var streamerController = new StreamerController(getStreamerService);

        var result = await streamerController.GetStreamer("");

        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        ErrorResponse errorResponse = Assert.IsType<ErrorResponse>(badRequestResult.Value);
        Assert.Equal("Streamer ID cannot be empty (Parameter 'streamerId')", errorResponse.Error);
    }

    [Fact]
    public async Task ErrorIfStreamerNotFound()
    {
        Mock<IStreamerManager> streamerManager = new Mock<IStreamerManager>();
        GetStreamerService getStreamerService = new GetStreamerService(streamerManager.Object);
        StreamerController streamerController = new StreamerController(getStreamerService);

        streamerManager.Setup(sm => sm.GetStreamer("1234")).ThrowsAsync(new KeyNotFoundException("Streamer with ID 1234 not found"));

        var result = await streamerController.GetStreamer("1234");

        NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        ErrorResponse errorResponse = Assert.IsType<ErrorResponse>(notFoundResult.Value);
        Assert.Equal("Streamer with ID 1234 not found", errorResponse.Error);
    }

    [Fact]
    public async Task GetsStreamer()
    {
        var twitchApiClient = new Mock<ITwitchApiClient>();
        var streamerManager = new StreamerManager(twitchApiClient.Object);
        var getStreamerService = new GetStreamerService(streamerManager);
        var streamerController = new StreamerController(getStreamerService);
        var expectedStreamer = new Streamer(
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

        twitchApiClient.Setup(tac => tac.GetUserByIdAsync("123")).ReturnsAsync(new TwitchResponse { Data = new List<Streamer> { expectedStreamer } });

        var result = await streamerController.GetStreamer("123");

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        Streamer streamer = Assert.IsType<Streamer>(okResult.Value);
        Assert.Equivalent(expectedStreamer, streamer);
    }
}
