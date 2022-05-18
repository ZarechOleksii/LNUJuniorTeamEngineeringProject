using Models.Entities;

namespace Data.CommentRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        public Task<bool> DeleteAllCommentsByUserAsync(User user);
        public Task<List<Comment>> GetAllMovieCommentsAsync(Guid movieId);
    }
}
