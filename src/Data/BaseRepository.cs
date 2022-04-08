using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext _context;
        protected readonly DbSet<T> _set;

        public BaseRepository(ApplicationContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public virtual async Task<bool> AddAsync(T entity)
        {
            _set.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            _set.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<T?> GetAsync(Guid id)
        {
            return await _set.FindAsync(id);
        }

        public virtual async Task<List<T>> FetchAll()
        {
            return await _set.ToListAsync();
        }

        public virtual async Task<List<T>> FetchAllNoTracking()
        {
            return await _set.AsNoTracking().ToListAsync();
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
