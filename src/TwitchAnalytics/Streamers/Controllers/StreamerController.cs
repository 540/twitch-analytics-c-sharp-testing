using Microsoft.AspNetCore.Mvc;
using TwitchAnalytics.Streamers.Infrastructure;
using TwitchAnalytics.Streamers.Models;
using TwitchAnalytics.Streamers.Services;

namespace TwitchAnalytics.Streamers.Controllers
{
    [ApiController]
    [Route("analytics/streamer")]
    public class StreamerController : ControllerBase
    {
        private readonly GetStreamerService service;

        public StreamerController(GetStreamerService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Streamer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStreamer([FromQuery] string id)
        {
            try
            {
                var streamer = await this.service.GetStreamer(id);
                return this.Ok(streamer);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(new ErrorResponse { Error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(new ErrorResponse { Error = ex.Message });
            }
            catch (Exception)
            {
                return this.StatusCode(500, new ErrorResponse { Error = "An unexpected error occurred while processing your request." });
            }
        }
    }
}
