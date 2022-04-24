using System;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Services.Implementations;
using Xunit;

namespace UnitTests.DataTests
{
    public class FavouriteServiceTest
    {
        private readonly BaseRepository<Favourites> _rep;
        private readonly ApplicationContext _context;
        private readonly FavouriteService _favouriteService;

        public FavouriteServiceTest()
        {
            var dbName = $"OnlyMovies_{DateTime.Now.ToFileTimeUtc()}";
            DbContextOptions<ApplicationContext> dbContextOptions
                = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            _context = new ApplicationContext(dbContextOptions);
            PopulateData(_context);
            _rep = new BaseRepository<Favourites>(_context);
            _favouriteService = new FavouriteService(_rep);
        }

        [Fact]
        public async Task AddToFavourite_Success()
        {
            // arrange
            var user_id = "user_id";
            var movie_id = Guid.NewGuid();

            var user = new User()
            {
                Id = user_id,
            };
            var movie = new Movie()
            {
                Id = movie_id,
            };
            var favourite = new Favourites()
            {
                UserId = user.Id,
                MovieId = movie.Id
            };

            // act
            await _favouriteService.AddToFavouriteAsync(favourite);
            await _rep.SaveChangesAsync();
            var new_favourites = await _rep.GetAsync(favourite.Id);

            // assert
            Assert.NotNull(new_favourites);
            Assert.Equal(movie_id, new_favourites.MovieId);
            Assert.Equal(user_id, new_favourites.UserId);
        }

        private static void PopulateData(ApplicationContext context)
        {
            context.BaseEntities.Add(new BaseEntity
            {
                Id = Guid.Parse("cefa6ffb-ce6e-4197-9c30-81459d072e5a")
            });
            context.BaseEntities.Add(new BaseEntity
            {
                Id = Guid.Parse("adc71289-a52b-4d06-9ffe-ed8d36604f13")
            });
            context.BaseEntities.Add(new BaseEntity
            {
                Id = Guid.Parse("a81e2adf-d628-45a2-ba9d-e30e1a337432")
            });
            context.BaseEntities.Add(new BaseEntity
            {
                Id = Guid.Parse("1fda063f-c6a1-4022-b2e3-6e4d43db4d33")
            });
            context.SaveChanges();
        }
    }
}
