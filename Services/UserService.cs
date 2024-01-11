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

        public UserService(IUserRepository userRepository, IMapper mapper )
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

        public Task<bool> DeleteUser(int? userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDto>> GetAllAvaiableContacts(UserDto? user)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> UpdateUser(UserDto? user)
        {
            throw new NotImplementedException();
        }
    }
}
