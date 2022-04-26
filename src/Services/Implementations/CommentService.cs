using Data;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Services.Interfaces;

namespace Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _repository;
        private readonly ILogger<CommentService> _logger;

        public CommentService(IRepository<Comment> rep, ILogger<CommentService> logger)
        {
            _repository = rep;
            _logger = logger;
        }

        public async Task<bool> AddCommentAsync(Comment comment)
        {
            try
            {
                return await _repository.AddAsync(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught exception in CommentService method AddCommentAsync");
                return false;
            }
        }
    }
}
