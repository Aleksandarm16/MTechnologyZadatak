using Entities;
using RepositoryContracts;

namespace Repositories
{
    public class ContactsRepository : IContactRepository
    {
        public Task<Contact> AddContact(Contact contact)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteContact(int contactId)
        {
            throw new NotImplementedException();
        }

        public Task<Contact> UpdateContact(Contact contact)
        {
            throw new NotImplementedException();
        }
    }
}
