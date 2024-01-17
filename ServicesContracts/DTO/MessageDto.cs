using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts.DTO
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? Sender { get; set; }
        public int? SenderId { get; set; }
        public string? Recipient { get; set; }
        public int? RecipientId { get;  set; }
    }
}
