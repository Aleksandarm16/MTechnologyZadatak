using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServicesContracts.DTO
{
    public class ContactDto
    {
        public int? ContactId { get; set; }
        public string? ContactName { get; set; }
        [Required(ErrorMessage = "Phone Nummber can't be blank")]
        public string? PhoneNummber { get; set; }
        [Required(ErrorMessage = "User Id can't be blank")]
        public int? UserId { get; set; }
    }
}
