using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserEmail { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
