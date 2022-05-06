using Models.Entities;

namespace Services.Interfaces
{
    public interface IHomeService
    {
        public Task<List<Movie>> GetMoviesPartAsync(int page, int pageSize);
    }
}
