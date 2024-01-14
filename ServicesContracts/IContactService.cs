using ServicesContracts.DTO;

namespace ServicesContracts
{
    public interface IContactService
    {
        /// <summary>
        /// Adds a new contact for the existing user to the database
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Returns the same contact details, along with newly generated ContactId</returns>
        Task<ContactDto> AddContact(ContactDto? contact);

        /// <summary>
        /// Updates the specified contact details based on the given Contact ID
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>Returns the contact response object after updating</returns>
        Task<ContactDto> UpdateContact(ContactDto? contact);

        /// <summary>
        ///  Deletes a contact based on the given contact id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns>Returns true, if the deletion is successful; otherwise false</returns>
        Task<bool> DeleteContact(int? contactId);

        /// <summary>
        /// Returns the contact object based on the given contact id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns matching contact object</returns>
        Task<ContactDto?> GetContactById(int? contactId);

        /// <summary>
        /// Returns all contacts
        /// </summary>
        /// <returns>Returns  a list of contacs dto objects</returns>
        Task<List<ContactDto>> GetAllContacts();
    }
}
