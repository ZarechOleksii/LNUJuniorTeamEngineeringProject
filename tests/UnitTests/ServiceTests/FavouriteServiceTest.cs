using System;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Moq;
using Services.Implementations;
using Xunit;

namespace UnitTests.DataTests
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
            var userId = Guid.NewGuid().ToString();
            var movieId = Guid.NewGuid();
            var mock = new Mock<IRepository<Favourites>>();

            mock.Setup(mock => mock.AddAsync(It.IsAny<Favourites>()))
                .Throws(new Exception());

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.AddToFavouriteAsync(userId, movieId);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteFromFavourites_WhenRepThrows_ReturnsFalse()
        {
            // arrange
            var userId = Guid.NewGuid().ToString();
            var movieId = Guid.NewGuid();
            var mock = new Mock<IRepository<Favourites>>();

            mock.Setup(mock => mock.DeleteAsync(It.IsAny<Favourites>()))
                .Throws(new Exception());

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.DeleteFromFavouriteAsync(userId, movieId);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddToFavourites_WhenRepFalse_ReturnsFalse()
        {
            // arrange
            var userId = Guid.NewGuid().ToString();
            var movieId = Guid.NewGuid();
            var mock = new Mock<IRepository<Favourites>>();

            mock.Setup(mock => mock.AddAsync(It.IsAny<Favourites>()))
                .ReturnsAsync(false);

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.AddToFavouriteAsync(userId, movieId);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteFromFavourites_WhenRepFalse_ReturnsFalse()
        {
            // arrange
            var userId = Guid.NewGuid().ToString();
            var movieId = Guid.NewGuid();
            var mock = new Mock<IRepository<Favourites>>();

            mock.Setup(mock => mock.DeleteAsync(It.IsAny<Favourites>()))
                .ReturnsAsync(false);

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.DeleteFromFavouriteAsync(userId, movieId);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddToFavourites_WhenRepTrue_ReturnsTrue()
        {
            // arrange
            var userId = Guid.NewGuid().ToString();
            var movieId = Guid.NewGuid();
            var mock = new Mock<IRepository<Favourites>>();

            mock.Setup(mock => mock.AddAsync(It.IsAny<Favourites>()))
                .ReturnsAsync(true);

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.AddToFavouriteAsync(userId, movieId);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteFromFavourites_WhenRepTrue_ReturnsTrue()
        {
            // arrange
            var userId = Guid.NewGuid().ToString();
            var movieId = Guid.NewGuid();
            var mock = new Mock<IRepository<Favourites>>();

            mock.Setup(mock => mock.DeleteAsync(It.IsAny<Favourites>()))
                .ReturnsAsync(true);

            _favouriteService = new (mock.Object, _mockedLogger.Object);

            // act
            var result = await _favouriteService.DeleteFromFavouriteAsync(userId, movieId);

            // assert
            Assert.True(result);
        }
    }
}
