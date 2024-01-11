using Entities;
using RepositoryContracts;

namespace Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UsersRepository(ApplicationDbContext db)
        {
                _db = db;
        }

        public async Task<User> AddUser(global::Entities.User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user;
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
