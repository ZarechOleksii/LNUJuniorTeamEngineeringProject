using System;
using System.Threading.Tasks;
using Data;
using Data.CommentRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Moq;
using Services.Implementations;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class CommentServiceTest
    {
        private readonly ILogger<CommentService> _mockedLogger;
        private readonly Mock<ICommentRepository> _repMock;
        private CommentService _commentService;

        public CommentServiceTest()
        {
            _mockedLogger = new Mock<ILogger<CommentService>>().Object;
            _repMock = new Mock<ICommentRepository>();
        }

        [Fact]
        public async Task AddComment_WhenRepThrows_ReturnsFalse()
        {
            // arrange
            var comment = SampleComment();

            _repMock.Setup(mock => mock.AddAsync(It.IsAny<Comment>()))
                .Throws(new Exception());

            _commentService = new (_repMock.Object, _mockedLogger);

            // act
            var result = await _commentService.AddCommentAsync(comment);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddComment_WhenRepFalse_ReturnsFalse()
        {
            // arrange
            var comment = SampleComment();

            _repMock.Setup(mock => mock.AddAsync(It.IsAny<Comment>()))
                .ReturnsAsync(false);

            _commentService = new (_repMock.Object, _mockedLogger);

            // act
            var result = await _commentService.AddCommentAsync(comment);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddComment_WhenRepTrue_ReturnsTrue()
        {
            // arrange
            var comment = SampleComment();

            _repMock.Setup(mock => mock.AddAsync(It.IsAny<Comment>()))
                .ReturnsAsync(true);

            _commentService = new (_repMock.Object, _mockedLogger);

            // act
            var result = await _commentService.AddCommentAsync(comment);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserCommentsAsync_WhenRepTrue_ReturnsTrue()
        {
            // arrange
            _repMock
                .Setup(mock => mock.DeleteAllCommentsByUserAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            _commentService = new (_repMock.Object, _mockedLogger);

            // act
            var result = await _commentService.DeleteUserCommentsAsync(new User());

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserCommentsAsync_WhenRepFalse_ReturnsFalse()
        {
            // arrange
            _repMock
                .Setup(mock => mock.DeleteAllCommentsByUserAsync(It.IsAny<User>()))
                .ReturnsAsync(false);

            _commentService = new (_repMock.Object, _mockedLogger);

            // act
            var result = await _commentService.DeleteUserCommentsAsync(new User());

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteUserCommentsAsync_WhenRepThrows_ReturnsFalse()
        {
            // arrange
            _repMock
                .Setup(mock => mock.DeleteAllCommentsByUserAsync(It.IsAny<User>()))
                .Throws(new Exception());

            _commentService = new (_repMock.Object, _mockedLogger);

            // act
            var result = await _commentService.DeleteUserCommentsAsync(new User());

            // assert
            Assert.False(result);
        }

        private static Comment SampleComment()
        {
            Comment comment = new ()
            {
                Content = "Test",
                UserId = Guid.NewGuid().ToString(),
                MovieId = Guid.NewGuid()
            };

            return comment;
        }
    }
}
