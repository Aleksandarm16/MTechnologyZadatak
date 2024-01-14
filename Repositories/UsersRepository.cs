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

        public async Task<List<User>> GetAllUsers()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User?> GetUserByUserId(int? userId)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.UserID == userId);
        }

        public async Task<User> UpdateUser(User user)
        {
            User? matchingUser = await _db.Users.FirstOrDefaultAsync(temp => temp.UserID == user.UserID);
            
            if (matchingUser == null)
            {
                return user;
            }
            matchingUser.PhoneNummber = user.PhoneNummber;
            matchingUser.UserName = user.UserName;
            matchingUser.Email = user.Email;

            int countUpdated = await _db.SaveChangesAsync();

            return matchingUser;
        }
    }
}
