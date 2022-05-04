using Models.Entities;

namespace Services.Interfaces
{
    public interface IMovieService
    {
        public Task<bool> AddMovieAsync(Movie movie);
        public Task<Movie?> GetMovieAsync(Guid movieId);
        public Task<bool> EditMovieAsync(Movie tracked, Movie untracked);
        public Task<bool> DeleteMovieAsync(Movie movie);
    }
}
