using Football_Project.Data.Entities.Abstact;
using System.Linq.Expressions;

namespace Football_Project.Data.Repositories.Services
{
    public interface IRepository { }
    public interface IRepository<T> : IRepository where T : IBaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
