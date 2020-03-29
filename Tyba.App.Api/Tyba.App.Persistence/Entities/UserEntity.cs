using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tyba.App.Persistence.Entities
{
    [Table("Users")]
    public class UserEntity
    {
        [Key]
        [Column("Email")]
        public string Email { get; set; }

        
        [Column("Password")]
        public string Password { get; set; }
    }
}
