using System;
using Football_Project.Data.Entities;
using Football_Project.Data.Repositories.Services;
using Microsoft.EntityFrameworkCore;

namespace Football_Project.Data.Repositories.Implementation
{
    public class MatchRepository : BaseRepository<Match>, IMatchRepository
    {
        public MatchRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Match>> GetMatchesByTeamIdAsync(int teamId)
        {
            return await _context.Matches
                .Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetAllMatches()
        {
            return await _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .ToListAsync();
        }

        public async Task<Match?> GetMatchById(int matchId)
        {
            return await _context.Matches
                .Where(m => m.MatchId == matchId)
                .Include(m => m.HomeTeam)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .FirstOrDefaultAsync();
        }

    }
}

