using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UserID { get; set; }

        [StringLength(40)]
        public string? UserName { get; set; }

        [StringLength(40)]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? PhoneNummber { get; set; }
    }
}
