using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        [StringLength(20)]
        public string? ContactName { get; set; }
        [StringLength(15)]
        public string? PhoneNummber { get; set;}

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User? User { get; set; }
    }
}
