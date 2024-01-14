using Microsoft.AspNetCore.Mvc;
using Services;
using ServicesContracts;
using ServicesContracts.DTO;

namespace ZadatakApi.Controllers
{
    [Route("[controller]")]
    public class ContactController : Controller
    {
        //private fields
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> CreateContact (ContactDto? contactDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(v=> v.Errors).Select(e=>e.ErrorMessage).ToList());
            }
            ContactDto responseContact = await _contactService.AddContact(contactDto);
            return Ok(responseContact);
        }

        [Route("[action]")]
        [HttpDelete]
        public async Task<IActionResult> DeleteContact(int? contactId)
        {
            if (contactId == null)
            {
                return BadRequest();
            }
            ContactDto? existing_contact = await _contactService.GetContactById(contactId);

            if (existing_contact == null)
            {
                return NotFound("Contact not found");
            }
            if(await _contactService.DeleteContact(contactId))
            {
                return Ok("Contact deleted");
            }
            else
            {
                return BadRequest("Something went wrong, can't delete contact");
            }
            
        }

        [HttpPost]
        [Route("[action]/{contactId}")]
        public async Task<IActionResult> EditContact(ContactDto? contact)
        {

            ContactDto? existing_contact = await _contactService.GetContactById(contact?.ContactId);

            if (existing_contact == null)
            {
                return BadRequest("Contact was not found");
            }
            if (ModelState.IsValid)
            {
                ContactDto updatedContact = await _contactService.UpdateContact(contact);
                return Ok(updatedContact);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _contactService.GetAllContacts());
        }

    }
}
