using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Task<bool> DeleteContact(int contactId);
    }
}
