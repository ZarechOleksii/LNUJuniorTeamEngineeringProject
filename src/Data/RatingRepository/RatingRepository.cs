using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.RatingRepository
{
    public class RatingRepository : BaseRepository<MovieRate>, IRatingRepository
    {
        public RatingRepository(ApplicationContext context)
            : base(context)
        {
        }

        public async Task<double> GetMovieRateAsync(Guid movieId)
        {
            return await _set.Where(i => i.MovieId == movieId).AverageAsync(p => p.Rate);
        }
    }
}
