using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;

namespace Services.Interfaces
{
    public interface ICommentService
    {
        public Task<bool> AddCommentAsync(Comment comment);
    }
}
