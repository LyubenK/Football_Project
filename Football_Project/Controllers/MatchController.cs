using Football_Project.Data.Models;
using Football_Project.Services.Exceptions;
using Football_Project.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Football_Project.Controllers
{
    [ApiController]
    [Route("api/match")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllMatches()
        {
            try
            {
                var matches = await _matchService.GetAllMatchesAsync();
                return Ok(matches);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            try
            {
                var match = await _matchService.GetMatchByIdAsync(id);
                if (match == null)
                {
                    return NotFound(new { error = $"Match with ID {id} not found." });
                }

                return Ok(match);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddMatch([FromBody] AddMatchViewModel match)
        {
            try
            {
                await _matchService.AddMatchAsync(match);
                return StatusCode(201, new { message = "Match created successfully." });
            }
            catch (CreationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> UpdateMatch(int id, [FromBody] UpdateMatchViewModel match)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(new { error = "Match ID mismatch." });
                }

                await _matchService.UpdateMatchAsync(id, match);
                return Ok(new { message = "Match updated successfully." });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (UpdateException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            try
            {
                await _matchService.DeleteMatchAsync(id);
                return Ok(new { message = "Match deleted successfully." });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (DeletionException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }

}