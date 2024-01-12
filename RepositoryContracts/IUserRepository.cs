using Entities;
using System;

namespace RepositoryContracts
{
    public interface IUserRepository
    {
        /// <summary>
        /// Filters all available contacts the user can chat with
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns list of contacts for the curent user </returns>
        Task<List<User>> GetAllAvaiableContacts(User? user);

        /// <summary>
        /// Adds a user object to the data table
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns the user object after adding it to the table</returns>
        Task<User> AddUser (User user);
        /// <summary>
        /// Update existing user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns updated user object</returns>

        Task<User> UpdateUser (User user);

        /// <summary>
        /// Delets user object based on the user id and all his contacts from the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns true if the deletion is successful</returns>
        Task<bool> DeleteUser (int? userId);

        /// <summary>
        /// Returns a user object based on the given user id
        /// </summary>
        /// <param name="personID">PersonID (guid) to search</param>
        /// <returns>A user object or null</returns>
        Task<User?> GetUserByUserId(int? userId);

    }
}
