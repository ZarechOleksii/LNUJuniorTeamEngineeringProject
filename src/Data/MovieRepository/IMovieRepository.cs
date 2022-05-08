using Models.Entities;

namespace Data.MovieRepository
{
    public interface IMovieRepository : IRepository<Movie>
    {
        public Task<Movie?> GetWithRelationsAsync(Guid id);
        public Task<List<Movie>> GetMoviesPart(int page, int pageSize);
    }
}
