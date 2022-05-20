using Models.Entities;

namespace Data.RatingRepository
{
    public interface IRatingRepository : IRepository<MovieRate>
    {
        public Task<double> GetMovieRateAsync(Guid movieId);
    }
}
