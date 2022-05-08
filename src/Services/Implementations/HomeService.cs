using Data.MovieRepository;
using Models.Entities;
using Services.Interfaces;

namespace Services.Implementations
{
    public class HomeService : IHomeService
    {
        private readonly IMovieRepository _repository;

        public HomeService(IMovieRepository rep)
        {
            _repository = rep;
        }

        public async Task<List<Movie>> GetMoviesPartAsync(int page, int pageSize)
        {
            return await _repository.GetMoviesPart(page, pageSize);
        }
    }
}
