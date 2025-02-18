using Moq;
using TwitchAnalytics.Streamers.Managers;
using TwitchAnalytics.Streamers.Models;
using TwitchAnalytics.Streamers.Services;
using Xunit;

namespace TwitchAnalytics.UnitTests.Streamers.Services
{
    public class GetStreamerServiceTest
    {
        [Fact]
        public async Task DoesNotGetStreameIfNoIdGiven()
        {
            Mock<IStreamerManager> streamerManager = new Mock<IStreamerManager>();
            var getStreamerService = new GetStreamerService(streamerManager.Object);

            await Assert.ThrowsAsync<ArgumentException>(() => getStreamerService.GetStreamer(""));

            streamerManager.Verify(sm => sm.GetStreamer(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetsStreamer()
        {
            var streamerManager = new Mock<IStreamerManager>();
            var getStreamerService = new GetStreamerService(streamerManager.Object);
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

            streamerManager.Setup(sm => sm.GetStreamer("123")).ReturnsAsync(expectedStreamer);

            var streamer = await getStreamerService.GetStreamer("123");

            streamerManager.Verify(sm => sm.GetStreamer("123"), Times.Once);
            Assert.Equal(expectedStreamer, streamer);
        }
    }
}
