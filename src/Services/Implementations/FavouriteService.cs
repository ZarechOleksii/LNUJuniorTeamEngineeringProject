using Data;
using Models.Entities;
using Services.Interfaces;

namespace Services.Implementations
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IRepository<Favourites> _repository;

        public FavouriteService(IRepository<Favourites> rep)
        {
            _repository = rep;
        }

        public async Task<bool> AddToFavouriteAsync(Favourites favourites)
        {
            return await _repository.AddAsync(favourites);
        }
    }
}
