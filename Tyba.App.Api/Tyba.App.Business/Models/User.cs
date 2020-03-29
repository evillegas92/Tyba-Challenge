using System.ComponentModel.DataAnnotations;

namespace Tyba.App.Business.Models
{
    public class User
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
