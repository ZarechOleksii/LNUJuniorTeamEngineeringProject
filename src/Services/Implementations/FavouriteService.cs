using Data;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Services.Interfaces;

namespace Services.Implementations
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteRepository _repository;
        private readonly ILogger<FavouriteService> _logger;

        public FavouriteService(IFavouriteRepository rep, ILogger<FavouriteService> logger)
        {
            _repository = rep;
            _logger = logger;
        }

        public async Task<bool> AddToFavouriteAsync(string userId, Guid movieId)
        {
            Favourites favourites = new ()
            {
                UserId = userId,
                MovieId = movieId
            };

            try
            {
                return await _repository.AddAsync(favourites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught exception in Favourite service AddToFavouritesAsync");
                return false;
            }
        }

        public async Task<bool> DeleteFromFavouriteAsync(string userId, Guid movieId)
        {
            Favourites? favourites = await _repository
                .FindByUserAndMovie(userId, movieId);

            if (favourites is null)
            {
                return false;
            }

            try
            {
                return await _repository.DeleteAsync(favourites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught exception in Favourite service DeleteFromFavouritesAsync");
                return false;
            }
        }
    }
}
