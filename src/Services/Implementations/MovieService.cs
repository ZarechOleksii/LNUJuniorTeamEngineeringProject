using Data.MovieRepository;
using Models.Entities;
using Services.Interfaces;

namespace Services.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;

        public MovieService(IMovieRepository rep)
        {
            _repository = rep;
        }

        public async Task<bool> AddMovieAsync(Movie movie)
        {
            return await _repository.AddAsync(movie);
        }

        public async Task<Movie?> GetMovieAsync(Guid movieId)
        {
            return await _repository.GetWithRelationsAsync(movieId);
        }
    }
}
