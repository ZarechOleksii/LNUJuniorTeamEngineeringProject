using Models.Entities;

namespace WebApp.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool Banned { get; set; }
        public List<Favourites> Favourites { get; set; }
    }
}
