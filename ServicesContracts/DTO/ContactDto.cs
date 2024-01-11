using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts.DTO
{
    public class ContactDto
    {
        public int ContactId { get; set; }
        public string? ContactName { get; set; }
        public string? PhoneNummber { get; set; }
        public virtual User? User { get; set; }
    }
}
