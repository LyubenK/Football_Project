using System;
namespace Football_Project.Data.Repositories.Services
{
    public interface IUnitOfWork : IDisposable
    {
        ITeamRepository Teams { get; }
        IMatchRepository Matches { get; }

        Task SaveAsync();
    }
}

