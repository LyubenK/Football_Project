using System;
using Football_Project.Data.Entities.Abstact;
using Football_Project.Data.Repositories.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Football_Project.Data.Repositories.Implementation
{
    public abstract class BaseRepository : IRepository { }
    public abstract class BaseRepository<T> : BaseRepository, IRepository<T>
        where T : class, IBaseEntity
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);
    }

}