using System;
using Football_Project.Data.Entities;
using Football_Project.Data.Models;
using Football_Project.Data.Repositories.Services;
using Football_Project.Services.Exceptions;
using Football_Project.Services.Services;
using Microsoft.Extensions.Logging;

namespace Football_Project.Services.Implementation
{
	public class MatchService : IMatchService
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MatchService> _logger;

        public MatchService(IUnitOfWork unitOfWork, ILogger<MatchService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task AddMatchAsync(AddMatchViewModel matchViewModel)
        {
            try
            {
                if (matchViewModel == null)
                {
                    throw new ArgumentNullException(nameof(matchViewModel), "Match cannot be null.");
                }

                if (matchViewModel.HomeTeamScore == null || matchViewModel.AwayTeamScore == null)
                {
                    throw new InvalidOperationException("Match must have scores to be considered played.");
                }

                var match = new Match
                {
                    HomeTeamId = matchViewModel.HomeTeamId,
                    AwayTeamId = matchViewModel.AwayTeamId,
                    HomeTeamScore = matchViewModel.HomeTeamScore,
                    AwayTeamScore = matchViewModel.AwayTeamScore
                };

                await _unitOfWork.Matches.AddAsync(match);
                await _unitOfWork.SaveAsync();

                await UpdateRankingsAsync(match);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid match data provided.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a match.");
                throw new CreationException("Failed to add the match.", ex);
            }
        }

        public async Task DeleteMatchAsync(int id)
        {
            try
            {
                var match = await _unitOfWork.Matches.GetByIdAsync(id);

                if (match == null)
                {
                    throw new EntityNotFoundException($"Match with ID {id} not found.");
                }

                await RevertRankingsAsync(match);

                _unitOfWork.Matches.Delete(match);
                await _unitOfWork.SaveAsync();
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the match with ID {id}.");
                throw new DeletionException($"Failed to delete the match with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<MatchViewModel>> GetAllMatchesAsync()
        {
            try
            {
                var matches = await _unitOfWork.Matches.GetAllMatches();
                if (matches == null || !matches.Any())
                {
                    throw new EntityNotFoundException("No matches found in the database.");
                }

                var matchViewModels = matches.Select(team => new MatchViewModel
                {
                    HomeTeamName = team.HomeTeam.Name ?? "Unknown team",
                    AeayTeamName = team.AwayTeam.Name ?? "Unknown team",
                    HomeTeamScore = team.HomeTeamScore,
                    AwayTeamScore = team.AwayTeamScore
                });

                return matchViewModels;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving matches.");
                throw new RetrievalException("Failed to retrieve matches.", ex);
            }
        }

        public async Task<MatchViewModel?> GetMatchByIdAsync(int id)
        {
            try
            {
                var match = await _unitOfWork.Matches.GetMatchById(id);
                if (match == null)
                {
                    throw new EntityNotFoundException($"Match with ID {id} not found.");
                }

                var matchViewModel =  new MatchViewModel
                {
                    HomeTeamName = match.HomeTeam.Name ?? "Unknown team",
                    AeayTeamName = match.AwayTeam.Name ?? "Unknown team",
                    HomeTeamScore =match.HomeTeamScore,
                    AwayTeamScore= match.AwayTeamScore
                };

                return matchViewModel;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the match with ID {id}.");
                throw new RetrievalException($"Failed to retrieve the match with ID {id}", ex);
            }
        }

        public async Task UpdateMatchAsync(int id, UpdateMatchViewModel match)
        {
            try
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match), "Match cannot be null.");
                }

                var existingMatch = await _unitOfWork.Matches.GetByIdAsync(id);

                if (existingMatch == null)
                {
                    throw new EntityNotFoundException($"Match with ID {id} not found.");
                }

                var homeTeam = await _unitOfWork.Teams.GetByIdAsync(existingMatch.HomeTeamId);
                var awayTeam = await _unitOfWork.Teams.GetByIdAsync(existingMatch.AwayTeamId);

                if (homeTeam == null || awayTeam == null)
                {
                    throw new EntityNotFoundException("One or both teams involved in the match were not found.");
                }

                await RevertRankingsAsync(existingMatch);

                existingMatch.HomeTeamScore = match.HomeTeamScore;
                existingMatch.AwayTeamScore = match.AwayTeamScore;

                await UpdateRankingsAsync(existingMatch);

                // Save updated match and team statistics
                _unitOfWork.Matches.Update(existingMatch);
                _unitOfWork.Teams.Update(homeTeam);
                _unitOfWork.Teams.Update(awayTeam);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the match with ID {id}.");
                throw new UpdateException($"Failed to update the match with ID {id}.", ex);
            }
        }


        private async Task UpdateRankingsAsync(Match match)
        {
            try
            {
                var homeTeam = await _unitOfWork.Teams.GetByIdAsync(match.HomeTeamId);
                var awayTeam = await _unitOfWork.Teams.GetByIdAsync(match.AwayTeamId);

                if (homeTeam == null || awayTeam == null)
                {
                    throw new EntityNotFoundException("One or both teams involved in the match were not found.");
                }

                if (match.HomeTeamScore > match.AwayTeamScore)
                {
                    homeTeam.Points += 3; 
                    homeTeam.Wins++;
                    awayTeam.Losses++;
                }
                else if (match.HomeTeamScore < match.AwayTeamScore)
                {
                    awayTeam.Points += 3; 
                    homeTeam.Losses++;
                    awayTeam.Wins++;
                }
                else
                {
                    homeTeam.Points += 1; 
                    awayTeam.Points += 1;
                    homeTeam.Draws++;
                    awayTeam.Draws++;
                }

                homeTeam.PlayedMatchesCount++;
                awayTeam.PlayedMatchesCount++;

                _unitOfWork.Teams.Update(homeTeam);
                _unitOfWork.Teams.Update(awayTeam);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating team rankings.");
                throw new UpdateException("Failed to update team rankings.", ex);
            }
        }

        private async Task RevertRankingsAsync(Match match)
        {
            try
            {
                var homeTeam = await _unitOfWork.Teams.GetByIdAsync(match.HomeTeamId);
                var awayTeam = await _unitOfWork.Teams.GetByIdAsync(match.AwayTeamId);

                if (homeTeam == null || awayTeam == null)
                {
                    throw new EntityNotFoundException("One or both teams involved in the match were not found.");
                }

                if (match.HomeTeamScore > match.AwayTeamScore)
                {
                    homeTeam.Points -= 3; 
                    homeTeam.Wins--;
                    awayTeam.Losses--;
                }
                else if (match.HomeTeamScore < match.AwayTeamScore)
                {
                    awayTeam.Points -= 3; 
                    homeTeam.Losses--;
                    awayTeam.Wins--;
                }
                else
                {
                    homeTeam.Points -= 1; 
                    awayTeam.Points -= 1;
                    homeTeam.Draws--;
                    awayTeam.Draws--;
                }

                homeTeam.PlayedMatchesCount--;
                awayTeam.PlayedMatchesCount--;

                _unitOfWork.Teams.Update(homeTeam);
                _unitOfWork.Teams.Update(awayTeam);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reverting team rankings.");
                throw new UpdateException("Failed to revert team rankings.", ex);
            }
        }
    }
}

