using Models.Entities;

namespace Data
{
    public interface IFavouriteRepository : IRepository<Favourites>
    {
        public Task<Favourites?> FindByUserAndMovie(string userId, Guid movieId);
    }
}
