using System.ComponentModel.DataAnnotations;

namespace RMSmax.Models.ViewModels
{
    public class LoginModel : MainViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; } = "/";

    }
}
