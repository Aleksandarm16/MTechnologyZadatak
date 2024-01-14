using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }
        [StringLength(20)]
        public string? ContactName { get; set; }
        [StringLength(15)]
        public string? PhoneNummber { get; set;}
        public int? UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User? User { get; set; }
    }
}
