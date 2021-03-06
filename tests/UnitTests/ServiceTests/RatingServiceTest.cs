using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.RatingRepository;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Moq;
using Services.Implementations;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class RatingServiceTest
    {
        private readonly ILogger<RatingService> _mockedLogger;
        private readonly Mock<IRatingRepository> _repositoryMock;
        private readonly RatingService _ratingService;

        public RatingServiceTest()
        {
            _mockedLogger = new Mock<ILogger<RatingService>>().Object;
            _repositoryMock = new Mock<IRatingRepository>();
            _ratingService = new (_repositoryMock.Object, _mockedLogger);
        }

        [Fact]
        public async Task AddMovieRate_WhenRepTrue_ReturnsTrue()
        {
            // arrange
            var rate = CreateRate();

            _repositoryMock.Setup(mock => mock.AddAsync(It.IsAny<MovieRate>())).ReturnsAsync(true);
            _repositoryMock.Setup(mock => mock.FetchAllNoTracking()).ReturnsAsync(new List<MovieRate>());

            // act
            var result = await _ratingService.AddRateAsync(rate);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task AddMovieRate_WhenRepFalse_ReturnsFalse()
        {
            // arrange
            var rate = CreateRate();

            _repositoryMock.Setup(mock => mock.AddAsync(It.IsAny<MovieRate>())).ReturnsAsync(false);
            _repositoryMock.Setup(mock => mock.FetchAllNoTracking()).ReturnsAsync(new List<MovieRate>());

            // act
            var result = await _ratingService.AddRateAsync(rate);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddMovieRate_WhenRepThrows_ReturnsFalse()
        {
            // arrange
            var rate = CreateRate();

            _repositoryMock.Setup(mock => mock.AddAsync(It.IsAny<MovieRate>())).Throws(new Exception());
            _repositoryMock.Setup(mock => mock.FetchAllNoTracking()).ReturnsAsync(new List<MovieRate>());

            // act
            var result = await _ratingService.AddRateAsync(rate);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateMovieRate_WhenRepTrue_ReturnsTrue()
        {
            // arrange
            var movieId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var existingRate = CreateRate(10, userId, movieId);

            _repositoryMock.Setup(mock => mock.AddAsync(It.IsAny<MovieRate>())).ReturnsAsync(true);
            _repositoryMock.Setup(mock => mock.FetchAllNoTracking()).ReturnsAsync(new List<MovieRate> { existingRate });

            var updatedRate = CreateRate(5, userId, movieId);

            // act
            var result = await _ratingService.AddRateAsync(updatedRate);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateMovieRate_WhenRepFalse_ReturnsFalse()
        {
            // arrange
            var movieId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var existingRate = CreateRate(10, userId, movieId);

            _repositoryMock.Setup(mock => mock.AddAsync(It.IsAny<MovieRate>())).ReturnsAsync(false);
            _repositoryMock.Setup(mock => mock.FetchAllNoTracking()).ReturnsAsync(new List<MovieRate> { existingRate });

            var updatedRate = CreateRate(5, userId, movieId);

            // act
            var result = await _ratingService.AddRateAsync(updatedRate);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateMovieRate_WhenRepThows_ReturnsFalse()
        {
            // arrange
            var movieId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var existingRate = CreateRate(10, userId, movieId);

            _repositoryMock.Setup(mock => mock.AddAsync(It.IsAny<MovieRate>())).Throws(new Exception());
            _repositoryMock.Setup(mock => mock.FetchAllNoTracking()).ReturnsAsync(new List<MovieRate> { existingRate });

            var updatedRate = CreateRate(5, userId, movieId);

            // act
            var result = await _ratingService.AddRateAsync(updatedRate);

            // assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(10.0, 10.0)]
        [InlineData(9.12980, 9.13)]
        [InlineData(0, 0)]
        public async Task GetMovieRate_WhenRepTrue_ReturnsRateValue(double averageRate, double expectedAverageRate)
        {
            // arrange
            _repositoryMock.Setup(mock => mock.GetMovieRateAsync(It.IsAny<Guid>())).ReturnsAsync(averageRate);

            // act
            var result = await _ratingService.GetRateAsync(default);

            // assert
            Assert.Equal(expectedAverageRate, result);
        }

        [Fact]
        public async Task GetMovieRate_WhenRepThrows_ReturnsZero()
        {
            // arrange
            _repositoryMock.Setup(mock => mock.GetMovieRateAsync(It.IsAny<Guid>())).Throws(new Exception());

            // act
            var result = await _ratingService.GetRateAsync(default);

            // assert
            Assert.Equal(0.0, result);
        }

        private static MovieRate CreateRate(
            byte? rate = null,
            string? userId = null,
            Guid? movieId = null)
        {
            return new MovieRate
            {
                Rate = rate ?? 1,
                UserId = userId ?? Guid.NewGuid().ToString(),
                MovieId = movieId ?? Guid.NewGuid()
            };
        }
    }
}
