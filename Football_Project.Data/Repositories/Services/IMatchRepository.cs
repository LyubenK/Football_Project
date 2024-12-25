using System;
using Football_Project.Data.Entities;

namespace Football_Project.Data.Repositories.Services
{
    public interface IMatchRepository : IRepository<Match>
    {
        Task<IEnumerable<Match>> GetMatchesByTeamIdAsync(int teamId);

        Task<IEnumerable<Match>> GetAllMatches();

        Task<Match?> GetMatchById(int matchId);
    }
}

