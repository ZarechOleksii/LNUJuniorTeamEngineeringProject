using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.MovieRepository
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationContext context)
            : base(context)
        {
        }

        public async Task<List<Movie>> GetMoviesPart(int page, int pageSize)
        {
            return await _set
                .OrderByDescending(r => r.Rating.Select(q => q.Rate).DefaultIfEmpty().Average(p => p))
                .ThenBy(n => n.Name)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Movie?> GetWithRelationsAsync(Guid id)
        {
            var movie = await _set
                .Include(movie => movie.Comments)
                .ThenInclude(q => q.User)
                .Include(movie => movie.Rating)
                .ThenInclude(rate => rate.User)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (movie is null)
            {
                return null;
            }

            await _context
               .Entry(movie)
               .Collection(q => q.Comments)
               .LoadAsync();

            return movie;
        }
    }
}
