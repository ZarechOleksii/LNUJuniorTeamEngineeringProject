using Models.Entities;

namespace Services.Interfaces
{
    public interface ICommentService
    {
        public Task<bool> AddCommentAsync(Comment comment);

        public Task<bool> DeleteCommentAsync(Comment comment);

        public Task<Comment?> GetCommentByIdAsync(Guid id);
    }
}
