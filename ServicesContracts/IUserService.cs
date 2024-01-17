using Entities;
using ServicesContracts.DTO;

namespace ServicesContracts
{
    public interface IUserService
    {
        /// <summary>
        /// Returns all contacts that logged user can send messages to
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<List<UserDto>> GetAllAvaiableContacts(UserDto? user);

        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns the same user details, along with newly generated UserID</returns>
        Task<UserDto> AddUser(UserDto? user);

        /// <summary>
        /// Updates the specified user details based on the given UserID
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns the user response object after updating</returns>
        Task<UserDto> UpdateUser(UserDto? user);

        /// <summary>
        ///  Deletes a user based on the given user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns true, if the deletion is successful; otherwise false</returns>
        Task<bool> DeleteUser (int? userId);


        /// <summary>
        /// Returns the user object based on the given user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns matching user object</returns>
        Task<UserDto?> GetUserById(int? id);


        /// <summary>
        /// Returns all users
        /// </summary>
        /// <returns>Returns  a list of user dto objects</returns>
        Task<List<UserDto>> GetAllUsers();

        /// <summary>
        /// Send message to existing Contact in sender phonebook
        /// </summary>
        /// <param name="message"></param>
        /// <returns>True if successful, false if failed</returns>
        Task<bool> SendMessageToContats(MessageDto? message);
    }
}
