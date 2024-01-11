using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts.DTO
{
    public class UserDto
    {
        public int? UserID { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNummber { get; set; }
    }
}
