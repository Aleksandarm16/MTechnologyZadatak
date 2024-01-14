using System.ComponentModel.DataAnnotations;

namespace ServicesContracts.DTO
{
    public class UserDto
    {
        public int? UserID { get; set; }
        [Required(ErrorMessage = "User Name can't be blank")]
        public string? UserName { get; set; }
        public string? Email { get; set; }
        [Required(ErrorMessage = "User Nummber can't be blank")]

        public string? PhoneNummber { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != typeof(UserDto))
            {
                return false;

            }
            UserDto userDto = (UserDto)obj;
            return UserID == userDto.UserID && UserName == userDto.UserName && Email == userDto.Email && userDto.PhoneNummber == PhoneNummber;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
