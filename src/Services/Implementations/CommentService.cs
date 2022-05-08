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

        public async Task<bool> DeleteCommentAsync(Comment comment)
        {
            try
            {
                return await _repository.DeleteAsync(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught exception in CommentService method DeleteCommentAsync");
                return false;
            }
        }

        public async Task<Comment?> GetCommentByIdAsync(Guid id)
        {
            try
            {
                return await _repository.GetAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught exception in CommentService method GetCommentByIdAsync");
                return null;
            }
        }
    }
}
