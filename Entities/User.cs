using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [StringLength(40)]
        public string? UserName { get; set; }

        [StringLength(40)]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? PhoneNummber { get; set; }
    }
}
