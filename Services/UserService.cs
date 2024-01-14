using AutoMapper;
using Entities;
using RepositoryContracts;
using Services.Helpers;
using ServicesContracts;
using ServicesContracts.DTO;

namespace Services
{
    public class UserService : IUserService
    {
        #region Private Fields

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        #endregion

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> AddUser(UserDto? userDto)
        {
            //check if User is not null
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }
            //Model validation
            ValidationHelper.ModelValidation(userDto);

            //convert UserDto to user type

            User user = _mapper.Map<User>(userDto);

            //add user to database

            User responseUser = await _userRepository.AddUser(user);

            //convert user to userDto and return User
            return _mapper.Map<UserDto>(_mapper.Map<UserDto>(responseUser));
        }

        public async Task<bool> DeleteUser(int? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            User? user = await _userRepository.GetUserByUserId(userId);
            if (user == null)
            {
                return false;
            }
            await _userRepository.DeleteUser(userId);

            return true;
        }

        public async Task<List<UserDto>> GetAllAvaiableContacts(UserDto? user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            User userToSearch = _mapper.Map<User>(user);
            User? existing_user = await _userRepository.GetUserByUserId(user.UserID);
            if (existing_user == null)
            {
                throw new ArgumentException("Given user id doesn't exist");
            }
            List<User> available_users = await _userRepository.GetAllAvaiableContacts(existing_user);
            if(available_users == null || available_users.Count.Equals(0))
            {
                return new List<UserDto>();
            }
            List<UserDto> result_users = _mapper.Map<List<UserDto>>(available_users);
            return result_users;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserById(int? userId)
        {
            if (userId == null)
            {
                return null;
            }
            User? user = await _userRepository.GetUserByUserId(userId);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUser(UserDto? userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }
            //validation
            ValidationHelper.ModelValidation(userDto);

            //Get matching person object to update
            User? mathingUser = await _userRepository.GetUserByUserId(userDto.UserID);
            if (mathingUser == null)
            {
                throw new ArgumentException("Given user id doesn't exist");
            }
            //update all details
            mathingUser.PhoneNummber = userDto.PhoneNummber;
            mathingUser.UserName = userDto.UserName;
            mathingUser.PhoneNummber = userDto.PhoneNummber;
            mathingUser.Email = userDto.Email;

            //Update
            await _userRepository.UpdateUser(mathingUser);

            return _mapper.Map<UserDto>(mathingUser);

        }
    }
}
