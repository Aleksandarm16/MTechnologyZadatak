using Entities;

namespace RepositoryContracts
{
    public interface IContactRepository
    {
        /// <summary>
        /// Adds contact for logged user
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Returns the contact that was added</returns>
        Task<Contact> AddContact(Contact contact);

        /// <summary>
        /// Updates existing contact of the logged user 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Returns updated contact</returns>
        Task<Contact> UpdateContact(Contact contact);   

        /// <summary>
        /// Deletes the selected contact for the logged user
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns>Returns true if the cotnact was deleted</returns>
        Task<bool> DeleteContact(int? contactId);

        /// <summary>
        /// Returns a contact object based on the given contact id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns>A contact object or null</returns>
        Task<Contact?> GetContactByContactId(int? contactId);

        /// <summary>
        /// Returns all contacts
        /// </summary>
        /// <returns>Returns a list of contatcs objects from the table</returns>
        Task<List<Contact>> GetAllContacts();
    }
}
