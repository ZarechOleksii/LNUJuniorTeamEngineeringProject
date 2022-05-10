using System;
using System.Threading.Tasks;
using Data;
using Data.FavouriteRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Moq;
using Services.Implementations;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class FavouriteServiceTest
    {
        private readonly Mock<ILogger<FavouriteService>> _mockedLogger;
        private FavouriteService _favouriteService;

        public FavouriteServiceTest()
        {
            _mockedLogger = new Mock<ILogger<FavouriteService>>();
        }

        [Fact]
        public async Task AddToFavourites_WhenRepThrows_ReturnsFalse()
        {
            // arrange
            var favourite = SampleFavourites();
            var mock = new Mock<IFavouriteRepository>();

            mock.Setup(mock => mock.AddAsync(It.IsAny<Favourites>()))
                .Throws(new Exception());

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.AddToFavouriteAsync(favourite.UserId, favourite.MovieId);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteFromFavourites_WhenRepThrows_ReturnsFalse()
        {
            // arrange
            var favourite = SampleFavourites();

            var mock = new Mock<IFavouriteRepository>();

            mock.Setup(mock => mock.FindByUserAndMovie(favourite.UserId, favourite.MovieId))
                .ReturnsAsync(favourite);

            mock.Setup(mock => mock.DeleteAsync(It.IsAny<Favourites>()))
                .Throws(new Exception());

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.DeleteFromFavouriteAsync(favourite.UserId, favourite.MovieId);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddToFavourites_WhenRepFalse_ReturnsFalse()
        {
            // arrange
            var favourite = SampleFavourites();
            var mock = new Mock<IFavouriteRepository>();

            mock.Setup(mock => mock.AddAsync(It.IsAny<Favourites>()))
                .ReturnsAsync(false);

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.AddToFavouriteAsync(favourite.UserId, favourite.MovieId);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteFromFavourites_WhenRepFalse_ReturnsFalse()
        {
            // arrange
            var favourite = SampleFavourites();
            var mock = new Mock<IFavouriteRepository>();

            mock.Setup(mock => mock.FindByUserAndMovie(favourite.UserId, favourite.MovieId))
                .ReturnsAsync(favourite);

            mock.Setup(mock => mock.DeleteAsync(It.IsAny<Favourites>()))
                .ReturnsAsync(false);

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.DeleteFromFavouriteAsync(favourite.UserId, favourite.MovieId);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddToFavourites_WhenRepTrue_ReturnsTrue()
        {
            // arrange
            var favourite = SampleFavourites();
            var mock = new Mock<IFavouriteRepository>();

            mock.Setup(mock => mock.AddAsync(It.IsAny<Favourites>()))
                .ReturnsAsync(true);

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.AddToFavouriteAsync(favourite.UserId, favourite.MovieId);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteFromFavourites_WhenRepTrue_ReturnsTrue()
        {
            // arrange
            var favourite = SampleFavourites();
            var mock = new Mock<IFavouriteRepository>();

            mock.Setup(mock => mock.FindByUserAndMovie(favourite.UserId, favourite.MovieId))
                .ReturnsAsync(favourite);

            mock.Setup(mock => mock.DeleteAsync(It.IsAny<Favourites>()))
                .ReturnsAsync(true);

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.DeleteFromFavouriteAsync(favourite.UserId, favourite.MovieId);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsAlreadyFavourites_WhenRepFound_ReturnsTrue()
        {
            // arrange
            var user = SampleUser();
            var movie = SampleMovie();
            var mock = new Mock<IFavouriteRepository>();

            mock.Setup(mock => mock.FindByUserAndMovie(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Favourites { UserId = user.Id, MovieId = movie.Id });

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.IsAlreadyFavouriteAsync(user.Id, movie.Id);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsAlreadyFavourites_WhenRepNotFound_ReturnsFalse()
        {
            // arrange
            var user = SampleUser();
            var movie = SampleMovie();
            var mock = new Mock<IFavouriteRepository>();

            mock.Setup(mock => mock.FindByUserAndMovie(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync((Favourites?)null);

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.IsAlreadyFavouriteAsync(user.Id, movie.Id);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsAlreadyFavourites_WhenRepThrows_ReturnsFalse()
        {
            // arrange
            var user = SampleUser();
            var movie = SampleMovie();
            var mock = new Mock<IFavouriteRepository>();

            mock.Setup(mock => mock.FindByUserAndMovie(It.IsAny<string>(), It.IsAny<Guid>()))
                .Throws(new Exception());

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.IsAlreadyFavouriteAsync(user.Id, movie.Id);

            // assert
            Assert.False(result);
        }

        private static Favourites SampleFavourites()
        {
            return new Favourites()
            {
                UserId = Guid.NewGuid().ToString(),
                MovieId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };
        }

        private static User SampleUser()
        {
            return new User()
            {
                Id = Guid.NewGuid().ToString()
            };
        }

        private static Movie SampleMovie()
        {
            return new Movie()
            {
                Id = Guid.NewGuid()
            };
        }
    }
}
