using Entities;
using RepositoryContracts;
using ServicesContracts;
using ServicesContracts.DTO;

namespace Services
{
    public class UserService : IUserService
    {
        #region Private Fields

        private readonly IUserRepository _userRepository;

        #endregion

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<UserDto> AddUser(UserDto? user)
        {
            throw new NotImplementedException();
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
