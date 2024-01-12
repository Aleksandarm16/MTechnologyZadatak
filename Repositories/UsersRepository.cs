using Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<User> AddUser(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUser(int? userId)
        {
            _db.Users.RemoveRange(_db.Users.Where(x => x.UserID == userId));
            int rowsDeleterd = await _db.SaveChangesAsync();

            return rowsDeleterd > 0;
        }

        public async Task<List<User>> GetAllAvaiableContacts(User? user)
        {
                var userContacts = await _db.Contacts
                    .Where(c => c.UserID == user.UserID)
                    .ToListAsync();

                var matchingContacts = await Task.Run(() => _db.Users
                    .AsEnumerable()
                    .Where(user => userContacts.Any(contact => contact.PhoneNummber == user.PhoneNummber))
                    .ToList()
                    );

                return matchingContacts;
        }

        public async Task<User?> GetUserByUserId(int? userId)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.UserID == userId);
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
