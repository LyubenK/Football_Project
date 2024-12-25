using Football_Project.Data.Models;
using Football_Project.Services.Exceptions;
using Football_Project.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Football_Project.Controllers
{
    [ApiController]
    [Route("api/teams")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("rankings")]
        public async Task<IActionResult> GetRankings()
        {
            try
            {
                var rankings = await _teamService.GetRankingsAsync();
                return Ok(rankings);
            }
            catch (RetrievalException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddTeam([FromBody] AddTeamViewModel team)
        {
            try
            {
                await _teamService.AddTeamAsync(team);
                return StatusCode(201, new { message = "Team created successfully." });
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
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] AddTeamViewModel team)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(new { error = "Team ID mismatch." });
                }

                await _teamService.UpdateTeamAsync(id, team);
                return Ok(new { message = "Team updated successfully." });
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
        public async Task<IActionResult> DeleteTeam(int id)
        {
            try
            {
                await _teamService.DeleteTeamAsync(id);
                return Ok(new { message = "Team deleted successfully." });
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
