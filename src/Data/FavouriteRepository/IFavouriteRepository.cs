using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;

namespace Data
{
    public interface IFavouriteRepository : IRepository<Favourites>
    {
        public Task<Favourites?> FindByUserAndMovie(string userId, Guid movieId);
    }
}
