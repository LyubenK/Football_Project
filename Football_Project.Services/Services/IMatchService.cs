using System;
using Football_Project.Data.Entities;
using Football_Project.Data.Models;

namespace Football_Project.Services.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchViewModel>> GetAllMatchesAsync();
        Task<MatchViewModel?> GetMatchByIdAsync(int id);
        Task AddMatchAsync(AddMatchViewModel match);
        Task UpdateMatchAsync(int id, UpdateMatchViewModel match);
        Task DeleteMatchAsync(int id);
    }
}

