﻿using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.MovieRepository
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationContext context)
            : base(context)
        {
        }

        public async Task<Movie?> GetWithRelationsAsync(Guid id)
        {
            var movie = await _set
                .Include(movie => movie.Comments)
                .ThenInclude(q => q.User)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (movie is null)
            {
                return movie;
            }

            await _context
               .Entry(movie)
               .Collection(q => q.Comments)
               .LoadAsync();

            return movie;
        }
    }
}
