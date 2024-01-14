using AutoMapper;
using Entities;
using RepositoryContracts;
using Services.Helpers;
using ServicesContracts;
using ServicesContracts.DTO;

namespace Services
{
    public class ContactService : IContactService
    {
        #region Private Fields

        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        #endregion

        public ContactService(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }
        public async Task<ContactDto> AddContact(ContactDto? contact)
        {
            //check if Contact is not null
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            //Model validation
            ValidationHelper.ModelValidation(contact);

            //convert ContactDto to contact type

            Contact contact_entity = _mapper.Map<Contact>(contact);

            //add contact to database

            Contact responseContact = await _contactRepository.AddContact(contact_entity);

            //convert contact to ContactDto and return Contact
            return _mapper.Map<ContactDto>(_mapper.Map<ContactDto>(responseContact));
        }

        public async Task<bool> DeleteContact(int? contactId)
        {
            if (contactId == null)
            {
                throw new ArgumentNullException(nameof(contactId));
            }
            Contact? contact = await _contactRepository.GetContactByContactId(contactId);
            if (contact == null)
            {
                return false;
            }
            await _contactRepository.DeleteContact(contactId);

            return true;
        }

        public async Task<List<ContactDto>> GetAllContacts()
        {
            var contacts = await _contactRepository.GetAllContacts();

            return _mapper.Map<List<ContactDto>>(contacts);
        }

        public async Task<ContactDto?> GetContactById(int? contactId)
        {
            if (contactId == null)
            {
                return null;
            }
            Contact? contact = await _contactRepository.GetContactByContactId(contactId);
            if (contact == null)
            {
                return null;
            }
            return _mapper.Map<ContactDto>(contact);
        }
         

        public async Task<ContactDto> UpdateContact(ContactDto? contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            //validation
            ValidationHelper.ModelValidation(contact);

            //Get matching contact object to update
            Contact? mathingContact = await _contactRepository.GetContactByContactId(contact.ContactId);
            if (mathingContact == null)
            {
                throw new ArgumentException("Given contact doesn't exist");
            }
            //update all details
            mathingContact.PhoneNummber = contact.PhoneNummber;
            mathingContact.ContactName = contact.ContactName;
            mathingContact.PhoneNummber = contact.PhoneNummber;

            //Update
            await _contactRepository.UpdateContact(mathingContact);

            return _mapper.Map<ContactDto>(mathingContact);
        }
    }
}
