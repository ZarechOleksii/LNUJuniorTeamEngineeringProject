using Models.Entities;

namespace Services.Interfaces
{
    public interface IFavouriteService
    {
        public Task<bool> AddToFavouriteAsync(Favourites favourites);
    }
}
