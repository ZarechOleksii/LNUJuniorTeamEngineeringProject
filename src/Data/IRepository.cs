namespace Data
{
    public interface IRepository<T> where T : class
    {
        public Task<T?> GetAsync(Guid id);
        public Task<bool> AddAsync(T entity);
        public Task<bool> DeleteAsync(T entity);
        public Task<List<T>> FetchAll();
        public Task<List<T>> FetchAllNoTracking();
        public Task<bool> SaveChangesAsync();
    }
}
