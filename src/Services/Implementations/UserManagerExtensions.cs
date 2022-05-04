using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Services.Implementations
{
    public static class UserManagerExtensions
    {
        public static async Task<User?> FindByIdWithRelations(
            this UserManager<User> input,
            string userId)
        {
            return await input.Users
                .Include(x => x.Favourites)
                .ThenInclude(fav => fav.Movie)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
