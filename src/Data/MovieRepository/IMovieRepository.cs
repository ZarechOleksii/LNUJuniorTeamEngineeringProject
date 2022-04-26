using Models.Entities;

namespace Data.MovieRepository
{
    public interface IMovieRepository : IRepository<Movie>
    {
        public Task<Movie?> GetWithRelationsAsync(Guid id);
    }
}
