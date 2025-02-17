using Microsoft.AspNetCore.Mvc;
using TwitchAnalytics.Models;
using TwitchAnalytics.Services;

namespace TwitchAnalytics.Controllers
{
    [ApiController]
    [Route("analytics")]
    public class UserController : ControllerBase
    {
        private readonly ITwitchService _twitchService;
        private readonly ILogger<UserController> _logger;

        public UserController(ITwitchService twitchService, ILogger<UserController> logger)
        {
            _twitchService = twitchService;
            _logger = logger;
        }

        [HttpGet("user")]
        [ProducesResponseType(typeof(TwitchUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser([FromQuery] string id)
        {
            try
            {
                var user = await _twitchService.GetUserByIdAsync(id);
                return Ok(user);
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
                _logger.LogError(ex, "Error occurred while fetching Twitch user");
                return StatusCode(500, new ErrorResponse { Error = "Internal server error." });
            }
        }
    }
}
