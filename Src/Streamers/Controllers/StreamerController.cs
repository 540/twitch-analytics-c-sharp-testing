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
        private readonly GetStreamerService _getStreamerService;
        private readonly ILogger<StreamerController> _logger;

        public StreamerController(GetStreamerService getStreamerService, ILogger<StreamerController> logger)
        {
            _getStreamerService = getStreamerService;
            _logger = logger;
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
                _logger.LogError("Controller " + id);
                Streamer streamer = await _getStreamerService.GetStreamer(id);
                return Ok(streamer);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse { Error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Twitch streamer");
                return StatusCode(500, new ErrorResponse { Error = "Internal server error." });
            }
        }
    }
}
