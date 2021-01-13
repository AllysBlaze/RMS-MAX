using System.ComponentModel.DataAnnotations;


namespace RMSmax.Models
{
    public class User
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }

    }
}
