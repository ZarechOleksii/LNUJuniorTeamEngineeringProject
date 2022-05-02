using Microsoft.AspNetCore.Identity;

namespace Models.Entities
{
    public class User : IdentityUser
    {
        public List<Favourites> Favourites { get; set; }
        public List<Comment> Comments { get; set; }

        public User()
        {
            Favourites = new List<Favourites>();
            Comments = new List<Comment>();
        }
    }
}
