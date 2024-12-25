using System;
using Football_Project.Data.Entities;

namespace Football_Project.Data.Repositories.Services
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<IEnumerable<Team>> GetRankingsAsync();
    }
}

