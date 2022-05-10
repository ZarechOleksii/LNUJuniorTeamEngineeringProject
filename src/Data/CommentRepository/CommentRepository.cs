﻿using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.CommentRepository
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationContext context)
            : base(context)
        {
        }

        public async Task<bool> DeleteAllCommentsByUserAsync(User user)
        {
            var userComments = _set.Where(c => c.UserId == user.Id);
            _set.RemoveRange(userComments);
            await SaveChangesAsync();
            return true;
        }
    }
}
