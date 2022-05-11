using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.FavouriteRepository
{
    public class FavouriteRepository : BaseRepository<Favourites>, IFavouriteRepository
    {
        public FavouriteRepository(ApplicationContext context)
            : base(context)
        {
        }

        public async Task<Favourites?> FindByUserAndMovie(string userId, Guid movieId)
        {
            return await _set
                .FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId);
        }
    }
}
