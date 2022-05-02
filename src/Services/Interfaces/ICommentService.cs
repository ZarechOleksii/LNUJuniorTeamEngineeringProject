using Models.Entities;

namespace Services.Interfaces
{
    public interface ICommentService
    {
        public Task<bool> AddCommentAsync(Comment comment);
    }
}
