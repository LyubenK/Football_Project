using System;
using Football_Project.Data.Entities;
using Football_Project.Data.Repositories.Services;
using Microsoft.EntityFrameworkCore;

namespace Football_Project.Data.Repositories.Implementation
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        public TeamRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Team>> GetRankingsAsync()
        {
            return await _context.Teams
                .OrderByDescending(t => t.Points)
                .ThenBy(t => t.Name)
                .ToListAsync();
        }
    }
}

