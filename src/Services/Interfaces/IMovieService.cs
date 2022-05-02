using Models.Entities;

namespace Services.Interfaces
{
    public interface IMovieService
    {
        public Task<bool> AddMovieAsync(Movie movie);
        public Task<Movie?> GetMovieAsync(Guid movieId);
    }
}
