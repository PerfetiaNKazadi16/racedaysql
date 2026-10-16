using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models1
{

    [Table("Users")]
    public class Users
    {
        [Key]
        [Column("usersID")]
        public int UsersId { get; set; }
        public string? UserName { get; set; }
        public string? role { get; set; }
        public string? Email { get; set; }
        public string? AccountType { get; set; }
        public string PasswordHash { get; set; } = string.Empty;

         
        public Organiser? Organiser { get; set; }

        public Participant? Participant { get; set; }
    }

}
