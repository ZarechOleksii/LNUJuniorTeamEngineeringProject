using Models.Entities;

namespace Data.FavouriteRepository
{
    public interface IFavouriteRepository : IRepository<Favourites>
    {
        public Task<Favourites?> FindByUserAndMovie(string userId, Guid movieId);
    }
}
