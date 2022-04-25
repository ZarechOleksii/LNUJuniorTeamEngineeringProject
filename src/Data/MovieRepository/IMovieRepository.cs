using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;

namespace Data.MovieRepository
{
    public interface IMovieRepository : IRepository<Movie>
    {
        public Task<Movie?> GetWithRelationsAsync(Guid id);
    }
}
