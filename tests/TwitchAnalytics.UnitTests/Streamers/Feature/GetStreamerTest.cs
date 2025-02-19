using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TwitchAnalytics.Streamers.Infrastructure;
using TwitchAnalytics.Streamers.Models;
using Xunit;

namespace TwitchAnalytics.UnitTests.Streamers.Feature;

public class GetStreamerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;
    private readonly Mock<ITwitchApiClient> mockTwitchClient;

    public GetStreamerTest(WebApplicationFactory<Program> factory)
    {
        mockTwitchClient = new Mock<ITwitchApiClient>();

        this.factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace the real API client with our mock
                services.AddScoped<ITwitchApiClient>(_ => this.mockTwitchClient.Object);
            });
        });
    }

    

    [Fact]
    public async Task DoesNotFoundStreamerWhenStreamerDoesNotExist()
    {
        // Arrange
        string streamerId = "nonexistent";
        mockTwitchClient
            .Setup(client => client.GetUserByIdAsync(streamerId))
            .ReturnsAsync(new TwitchResponse { Data = new List<Streamer>() });
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync($"/analytics/streamer?id={streamerId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        ErrorResponse? error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal($"Streamer with ID {streamerId} not found", error?.Error);
    }

    [Fact]
    public async Task GetsStreamer()
    {
        string streamerId = "123";
        Streamer expectedStreamer = new Streamer(
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
        mockTwitchClient
            .Setup(client => client.GetUserByIdAsync(streamerId))
            .ReturnsAsync(new TwitchResponse { Data = new List<Streamer> { expectedStreamer } });
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync($"/analytics/streamer?id={streamerId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Streamer? streamer = await response.Content.ReadFromJsonAsync<Streamer>();
        Assert.Equivalent(expectedStreamer, streamer);
    }
}
