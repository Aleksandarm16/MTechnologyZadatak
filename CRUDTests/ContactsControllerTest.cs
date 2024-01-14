using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services;
using ServicesContracts;
using ServicesContracts.DTO;
using Xunit;
using ZadatakApi.Controllers;

namespace CRUDTests
{
    public class ContactsControllerTest
    {
        //private fields
        private readonly IContactService _contactService;
        private readonly Mock<IContactService> _contactServiceMock;
        private readonly Fixture _fixture;

        public ContactsControllerTest()
        {
            _fixture = new Fixture();
            _contactServiceMock = new Mock<IContactService>();
            _contactService = _contactServiceMock.Object;
        }

        #region Create
        //If the ContactDto has some missing information bad request should be thrown
        [Fact]
        public async void CreateContact_IfModelErrors_ToReturnBadRequest()
        {
            //Arrange
            ContactDto contact_add_request = _fixture.Create<ContactDto>();
            ContactDto contact_response = _fixture.Create<ContactDto>();

            _contactServiceMock
            .Setup(temp => temp.AddContact(It.IsAny<ContactDto>()))
                .ReturnsAsync(contact_response);

            ContactController contactController = new ContactController(_contactService);

            //Act
            contactController.ModelState.AddModelError("PhoneNummber", "User phone nummber can't be blank");

            IActionResult result = await contactController.CreateContact(contact_add_request);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        //If we supply valid ContactDto object we should get Ok object result from controller
        [Fact]
        public async void CreateContact_ShouldReturnOkRequest()
        {
            //Arrange
            ContactDto contact_add_request = _fixture.Create<ContactDto>();
            ContactDto contact_response = _fixture.Create<ContactDto>();

            _contactServiceMock
            .Setup(temp => temp.AddContact(It.IsAny<ContactDto>()))
                .ReturnsAsync(contact_response);

            ContactController contactController = new ContactController(_contactService);

            //Act
            IActionResult result = await contactController.CreateContact(contact_add_request);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region Delete

        //If we supply invalid Id should return Bad Request
        [Fact]
        public async Task DeleteContact_InvalidID()
        {
            //Arrange
            int? contactId = null;

            ContactController contactController = new ContactController(_contactService);

            //Act
            IActionResult result = await contactController.DeleteContact(contactId);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }
        //If we supply valid Id and contact was not found it should return NotFound
        [Fact]
        public async Task DeleteContact_ValidID_ReturnsNotFound()
        {
            //Arrange
            int? contactId = 3;
            ContactDto? contact_response = null;

            ContactController contactController = new ContactController(_contactService);
            _contactServiceMock
                .Setup(temp => temp.GetContactById(contactId))
                .ReturnsAsync(contact_response);

            _contactServiceMock
                .Setup(temp => temp.DeleteContact(contactId))
                .ReturnsAsync(false);

            //Act
            IActionResult result = await contactController.DeleteContact(contactId);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        //If we supply valid Id and contact was found it should return Ok
        [Fact]
        public async Task DeleteContact_ValidID_ReturnsOk()
        {
            //Arrange
            int? contactId = 3;
            ContactDto? contact_response = _fixture.Create<ContactDto>();

            ContactController contactController = new ContactController(_contactService);
            _contactServiceMock
                .Setup(temp => temp.GetContactById(contactId))
                .ReturnsAsync(contact_response);
            _contactServiceMock
                .Setup(temp => temp.DeleteContact(contactId))
                .ReturnsAsync(true);

            //Act
            IActionResult result = await contactController.DeleteContact(contactId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region EditContact

        //If we supply invalid ContactID it should return badrequest object

        [Fact]
        public async Task EditContact_InvalidId_ToReturn_BadRequestObject()
        {
            //Arrange
            ContactDto? contact = null;
            ContactController contactController = new ContactController(_contactService);

            //Act
            IActionResult result = await contactController.EditContact(contact);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        //If the model state is valid it should update the contact
        [Fact]
        public async Task EditContact_ValidId_ToReturn_OkObjectResult()
        {
            ContactDto? contact_post = _fixture.Create<ContactDto>();
            ContactController contactController = new ContactController(_contactService);
            _contactServiceMock
                .Setup(temp => temp.GetContactById(contact_post.ContactId))
                .ReturnsAsync(contact_post);
            _contactServiceMock
                .Setup(temp => temp.UpdateContact(contact_post))
                .ReturnsAsync(contact_post);

            //Act
            IActionResult result = await contactController.EditContact(contact_post);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        //If the model state is not valid it should return Bad request Object
        [Fact]
        public async Task EditContact_ValidId_ToReturn_BadRequistObject()
        {
            ContactDto? contact_post = _fixture.Create<ContactDto>();
            ContactController contactController = new ContactController(_contactService);

            //Act
            contactController.ModelState.AddModelError("PhoneNummber", "Contact phone nummber can't be blank");
            IActionResult result = await contactController.EditContact(contact_post);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion
    }
}
