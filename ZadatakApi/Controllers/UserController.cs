using Microsoft.AspNetCore.Mvc;
using ServicesContracts;
using ServicesContracts.DTO;

namespace ZadatakApi.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        //private fields
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
                _userService = userService;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task <IActionResult> Create(UserDto userDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e=> e.ErrorMessage).ToList());
            }

            UserDto responseUser = await _userService.AddUser(userDto);

            return Ok(responseUser);
        }
    }
}
