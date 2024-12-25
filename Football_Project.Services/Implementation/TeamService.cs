using Football_Project.Data.Entities;
using Football_Project.Data.Models;
using Football_Project.Data.Repositories.Services;
using Football_Project.Services.Exceptions;
using Football_Project.Services.Services;
using Microsoft.Extensions.Logging;

namespace Football_Project.Services.Implementation
{
    public class TeamService : ITeamService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TeamService> _logger;

        public TeamService(IUnitOfWork unitOfWork, ILogger<TeamService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task AddTeamAsync(AddTeamViewModel teamViewModel)
        {
            try
            {
                if (teamViewModel == null)
                {
                    throw new ArgumentNullException(nameof(teamViewModel), "Team view model cannot be null.");
                }

                var team = new Team
                {
                    Name = teamViewModel.Name
                };

                await _unitOfWork.Teams.AddAsync(team);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a team.");
                throw new CreationException("Failed to add the team.", ex);
            }
        }
        public async Task DeleteTeamAsync(int id)
        {
            try
            {
                var team = await _unitOfWork.Teams.GetByIdAsync(id);

                if (team == null)
                {
                    throw new EntityNotFoundException($"Team with ID {id} not found.");
                }

                _unitOfWork.Teams.Delete(team);
                await _unitOfWork.SaveAsync();
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the team with ID {id}.");
                throw new DeletionException($"Failed to delete the team with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<TeamViewModel>> GetRankingsAsync()
        {
            try
            {
                var teams = await _unitOfWork.Teams.GetRankingsAsync();
                if (teams == null || !teams.Any())
                {
                    throw new EntityNotFoundException("No teams found in the database.");
                }

                var teamViewModels = teams.Select(team => new TeamViewModel
                {
                    Name = team.Name,
                    Points = team.Points,
                    PlayedMatchesCount = team.PlayedMatchesCount,
                    Wins = team.Wins,
                    Draws = team.Draws,
                    Losses = team.Losses,
                });

                return teamViewModels;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving team rankings.");
                throw new ServiceException("Failed to retrieve team rankings.", ex);
            }
        }

        public async Task UpdateTeamAsync(int id, AddTeamViewModel teamViewModel)
        {
            try
            {
                if (teamViewModel == null)
                {
                    throw new ArgumentNullException(nameof(teamViewModel), "Team view model cannot be null.");
                }

                var existingTeam = await _unitOfWork.Teams.GetByIdAsync(id);

                if (existingTeam == null)
                {
                    throw new EntityNotFoundException($"Team with ID {id} not found.");
                }


                existingTeam.Name = teamViewModel.Name;

                _unitOfWork.Teams.Update(existingTeam);
                await _unitOfWork.SaveAsync();
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the team with ID {id}.");
                throw new UpdateException($"Failed to update the team with ID {id}.", ex);
            }
        }
    }
}
