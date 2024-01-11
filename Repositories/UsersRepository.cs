using RepositoryContracts;

namespace Repositories
{
    public class UsersRepository : IUserRepository
    {
        public Task<global::Entities.User> AddUser(global::Entities.User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<global::Entities.User> GetAllAvailableUsers(global::Entities.User user)
        {
            throw new NotImplementedException();
        }

        public Task<global::Entities.User> UpdateUser(global::Entities.User user)
        {
            throw new NotImplementedException();
        }
    }
}
