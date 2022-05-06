using Data.MovieRepository;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Services.Interfaces;

namespace Services.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;
        private readonly ILogger<MovieService> _logger;

        public MovieService(IMovieRepository rep, ILogger<MovieService> logger)
        {
            _repository = rep;
            _logger = logger;
        }

        public async Task<bool> AddMovieAsync(Movie movie)
        {
            return await _repository.AddAsync(movie);
        }

        public async Task<Movie?> GetMovieAsync(Guid movieId)
        {
            return await _repository.GetWithRelationsAsync(movieId);
        }

        public async Task<bool> EditMovieAsync(Movie tracked, Movie untracked)
        {
            try
            {
                tracked.Description = untracked.Description;
                tracked.Name = untracked.Name;
                tracked.Url = untracked.Url;

                return await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught exception in MovieService method EditMovieAsync");
                return false;
            }
        }

        public async Task<bool> DeleteMovieAsync(Movie movie)
        {
            try
            {
                return await _repository.DeleteAsync(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught exception in MovieService method DeleteMovieAsync");
                return false;
            }
        }
    }
}
