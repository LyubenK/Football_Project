using System;
using Football_Project.Data.Entities;
using Football_Project.Data.Models;

namespace Football_Project.Services.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamViewModel>> GetRankingsAsync();
        //Task<Team?> GetTeamByIdAsync(int id);       
        Task AddTeamAsync(AddTeamViewModel team);                
        Task UpdateTeamAsync(int id, AddTeamViewModel team);             
        Task DeleteTeamAsync(int id);                
    }

}

