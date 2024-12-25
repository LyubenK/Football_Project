using System;
using Football_Project.Data.Repositories.Services;

namespace Football_Project.Data.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ITeamRepository Teams { get; }
        public IMatchRepository Matches { get; }

        public UnitOfWork(ApplicationDbContext context, ITeamRepository teams, IMatchRepository matches)
        {
            _context = context;
            Teams = teams;
            Matches = matches;
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

