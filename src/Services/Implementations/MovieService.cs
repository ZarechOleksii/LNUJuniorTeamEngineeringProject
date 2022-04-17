using Data;
using Models.Entities;
using Services.Interfaces;

namespace Services.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _repository;

        public MovieService(IRepository<Movie> rep)
        {
            _repository = rep;
        }

        public async Task<bool> AddMovieAsync(Movie movie)
        {
            return await _repository.AddAsync(movie);
        }
    }
}
