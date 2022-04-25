using Models.Entities;

namespace Services.Interfaces
{
    public interface IFavouriteService
    {
        public Task<bool> AddToFavouriteAsync(string userId, Guid movieId);
        public Task<bool> DeleteFromFavouriteAsync(string userId, Guid movieId);
    }
}
