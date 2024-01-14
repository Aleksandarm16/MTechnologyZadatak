using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class ContactsRepository : IContactRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactsRepository(ApplicationDbContext db)
        {
                _db= db;
        }
        public async Task<Contact> AddContact(Contact contact)
        {
            _db.Contacts.Add(contact);
            await _db.SaveChangesAsync();

            return contact;
        }

        public async Task<bool> DeleteContact(int? contactId)
        {
            _db.Contacts.RemoveRange(_db.Contacts.Where(x => x.ContactId == contactId));
            int rowsDeleterd = await _db.SaveChangesAsync();

            return rowsDeleterd > 0;
        }

        public async Task<List<Contact>> GetAllContacts()
        {
            return await _db.Contacts.Include("User").ToListAsync();
        }

        public async Task<Contact?> GetContactByContactId(int? contactId)
        {
            return await _db.Contacts.FirstOrDefaultAsync(x => x.ContactId == contactId);
        }

        public async Task<Contact> UpdateContact(Contact contact)
        {
            Contact? matchingContact = await _db.Contacts.FirstOrDefaultAsync(temp => temp.ContactId == contact.ContactId);

            if (matchingContact == null)
            {
                return contact;
            }
            matchingContact.PhoneNummber = contact.PhoneNummber;
            matchingContact.ContactName = contact.ContactName;
            matchingContact.UserID = contact.UserID;

            int countUpdated = await _db.SaveChangesAsync();

            return matchingContact;
        }
    }
}
