using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
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
        public async Task <IActionResult> CreateUser(UserDto userDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e=> e.ErrorMessage).ToList());
            }

            UserDto responseUser = await _userService.AddUser(userDto);

            return Ok(responseUser);
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetAvailableContacts (int? userId)
        {
            if(userId == null)
            {
                return BadRequest();
            }

            UserDto? existing_user = await _userService.GetUserById(userId);

            if(existing_user == null)
            {
                return NotFound();
            }

            List<UserDto> mathingContacts = await _userService.GetAllAvaiableContacts(existing_user);

            return Ok(mathingContacts);

        }

        [Route("[action]")]
        [HttpDelete]
        public async Task <IActionResult> DeleteUser (int? userId)
        {
            if(userId==null)
            {
                return BadRequest();
            }
            UserDto? existing_user = await _userService.GetUserById(userId);

            if (existing_user == null)
            {
                return NotFound();
            }

            if (await _userService.DeleteUser(userId))
            {
                return Ok("User deleted");
            }
            else
            {
                return BadRequest("Something went wrong, can't delete contact");
            }
        }

        [HttpPost]
        [Route("[action]/{userId}")]
        public async Task <IActionResult> EditUser (UserDto? user)
        {

            UserDto? existing_user = await _userService.GetUserById(user?.UserID);

            if (existing_user == null)
            {
                return BadRequest("User was not found");
            }
            if(ModelState.IsValid)
            {
                UserDto updatedUser = await _userService.UpdateUser(user);
                return Ok(updatedUser);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto? message)
        {
            bool result = await _userService.SendMessageToContats(message);
            if(result)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
