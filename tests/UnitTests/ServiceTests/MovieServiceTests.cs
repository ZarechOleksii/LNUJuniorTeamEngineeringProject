using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.MovieRepository;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Moq;
using Services.Implementations;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class MovieServiceTests
    {
        private readonly Mock<IMovieRepository> _repMock;
        private readonly ILogger<MovieService> _mockedLogger;
        private MovieService _movieService;

        public MovieServiceTests()
        {
            _mockedLogger = new Mock<ILogger<MovieService>>().Object;
            _repMock = new Mock<IMovieRepository>();
        }

        [Fact]
        public async Task EditMovieAsync_WhenRepReturnsTrue_ReturnsTrue()
        {
            // arrange
            _repMock
                .Setup(q => q.SaveChangesAsync())
                .ReturnsAsync(true);

            _movieService = new MovieService(_repMock.Object, _mockedLogger);

            // act
            var result = await _movieService.EditMovieAsync(SampleMovie(), SampleMovie());

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task EditMovieAsync_WhenRepReturnsFalse_ReturnsFalse()
        {
            // arrange
            _repMock
                .Setup(q => q.SaveChangesAsync())
                .ReturnsAsync(false);

            _movieService = new MovieService(_repMock.Object, _mockedLogger);

            // act
            var result = await _movieService.EditMovieAsync(SampleMovie(), SampleMovie());

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task EditMovieAsync_WhenRepThrows_ReturnsFalse()
        {
            // arrange
            _repMock
                .Setup(q => q.SaveChangesAsync())
                .ThrowsAsync(new Exception());

            _movieService = new MovieService(_repMock.Object, _mockedLogger);

            // act
            var result = await _movieService.EditMovieAsync(SampleMovie(), SampleMovie());

            // assert
            Assert.False(result);
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
