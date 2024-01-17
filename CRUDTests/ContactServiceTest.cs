using AutoFixture;
using AutoMapper;
using Moq;
using RepositoryContracts;
using Services.EntityProfiler;
using Services;
using ServicesContracts;
using Entities;
using FluentAssertions;
using ServicesContracts.DTO;
using Xunit;

namespace CRUDTests
{
    public class ContactServiceTest
    {
        #region Private Fields

        private readonly IContactService _contactService;
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly IFixture _fixture;

        #endregion

        public ContactServiceTest()
        {
            _fixture = new Fixture();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _contactRepository = _contactRepositoryMock.Object;
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(AppMappingProfile));
            });
            var mapper = mapperConfiguration.CreateMapper();
            _mapper = mapper;

            _contactService = new ContactService(_contactRepository, _mapper);
        }
        #region Add Conact
        //If the contact is invalid it should throw argument null exception
        [Fact]
        public async Task AddContact_NullContact_ToBeArgumentNullException()
        {
            //Arrange
            ContactDto? contactDto = null;

            //Act
            Func<Task> action = async () =>
            {
                await _contactService.AddContact(contactDto);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        //If the contact phone nummber is invalid it should throw argument null exception
        public async Task AddContact_UserPhoneIsNull_ToBeArgumentException()
        {
            //Arrange

            ContactDto? contactDto = new ContactDto { ContactName="test12" };
            Contact contact = _mapper.Map<Contact>(contactDto);

            _contactRepositoryMock.Setup(temp => temp.AddContact(It.IsAny<Contact>()))
                .ReturnsAsync(contact);

            //Act
            Func<Task> action = async () =>
            {
                await _contactService.AddContact(contactDto);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        // If we provide all information it should be successful
        public async Task AddContact_FullContactDetails_ToBeSuccessful()
        {
            //Arrange
            ContactDto contactDto = _fixture.Create<ContactDto>();

            Contact contact = _mapper.Map<Contact>(contactDto);
            ContactDto expectedContact = _mapper.Map<ContactDto>(contact);

            _contactRepositoryMock.Setup(temp => temp.AddContact(It.IsAny<Contact>()))
                .ReturnsAsync(contact);

            //Act

            ContactDto contactDtoResponse = await _contactService.AddContact(contactDto);

            //Assert
            contactDtoResponse.Should().BeEquivalentTo(expectedContact);
        }

        #endregion

        #region Update Contact

        //When we supply null as Contact object it should throw Argument Null Exception
        [Fact]
        public async Task UpdateContact_NullContact_ToBeArgumentNullException()
        {
            //Arrange
            ContactDto? contactDto = null;

            //Act
            Func<Task> action = async () =>
            {
                await _contactService.UpdateContact(contactDto);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply invalid conact id, it should throw Argument Exception
        [Fact]
        public async Task UpdateContact_InvalidConactId_ToBeArgumentException()
        {
            //Arrange
            ContactDto? contactDto = _fixture.Create<ContactDto>();

            //Act
            Func<Task> action = async () =>
            {
                await _contactService.UpdateContact(contactDto);
            };

            //Assert 
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //First, add a new contact and try to update the phone nummber and contact name

        [Fact]
        public async Task UpdateContact_ContactFullDetails_ToBeSuccessfull()
        {
            //Arrange
            ContactDto contactDto = _fixture.Build<ContactDto>()
                .With(x => x.PhoneNummber, "+381652323")
                .With(x => x.ContactName, "testcontact")
                .With(x => x.ContactId, 2)
                .Create();

            Contact contact = _mapper.Map<Contact>(contactDto);

            _contactRepositoryMock.Setup(x => x.GetContactByContactId(contactDto.ContactId))
                .ReturnsAsync(contact);
            _contactRepositoryMock
                .Setup(temp => temp.UpdateContact(It.IsAny<Contact>()))
                .ReturnsAsync(contact);

            //Act
            ContactDto responseContact = await _contactService.UpdateContact(contactDto);

            //Assert
            responseContact.Should().BeEquivalentTo(contactDto);
        }

        #endregion

        #region DeleteContact

        //If you supply an valid conact Id, it should return true
        [Fact]
        public async Task DeleteContact_ValidContactId_ToBeSuccessful()
        {
            //Arrange
            Contact contact = _fixture.Build<Contact>()
                .With(x => x.UserID, 2)
                .With(x => x.ContactName, "test1")
                .With(x => x.PhoneNummber, "+38167233")
                .With(x => x.ContactId, 1)
                .With(x => x.User, null as User)
                .Create();

            _contactRepositoryMock.Setup(x => x.DeleteContact(It.IsAny<int>()))
                .ReturnsAsync(true);

            _contactRepositoryMock
                .Setup(x => x.GetContactByContactId(It.IsAny<int>()))
                .ReturnsAsync(contact);

            //Act
            bool isDeleted = await _contactService.DeleteContact(contact.ContactId);

            //Assert
            isDeleted.Should().BeTrue();
        }

        // If you supply an invalid contact Id it should return false
        [Fact]
        public async Task DeleteContact_InvalidContactId()
        {
            //Arrange
            Contact contact = _fixture.Build<Contact>()
                .With(x => x.UserID, 2)
                .With(x => x.ContactName, "test1")
                .With(x => x.PhoneNummber, "+38167233")
                .With(x => x.ContactId, 3)
                .With(x => x.User, null as User)
                .Create();

            _contactRepositoryMock.Setup(x => x.DeleteContact(1))
                .ReturnsAsync(true);

            _contactRepositoryMock
                .Setup(x => x.GetContactByContactId(1))
                .ReturnsAsync(contact);
            //Act
            bool isDeleted = await _contactService.DeleteContact(contact.ContactId);

            //Assert
            isDeleted.Should().BeFalse();
        }

        #endregion

        #region Get Contact By Contact ID
        //If we supply null as contactId it should retun null as response
        [Fact]
        public async Task GetContactByContactId_NullContactId_ToBeNull()
        {
            //Arrange
            int? contactId = null;

            //Act
            ContactDto? contactDto = await _contactService.GetContactById(contactId);

            //Assert
            contactDto.Should().BeNull();
        }

        //If we supply a valid contact id, it should return the valid contact details as ContactDto object
        [Fact]
        public async Task GetContactByContactId_WithContactId_ToBeSuccessful()
        {
            //Arange
            Contact contact = _fixture.Build<Contact>()
                .With(temp => temp.User, null as User)
                .With(temp => temp.ContactId, 1)
                .Create();

            ContactDto contactDto = _mapper.Map<ContactDto>(contact);

            _contactRepositoryMock.Setup(x => x.GetContactByContactId(contact.ContactId))
                .ReturnsAsync(contact);

            //Act
            ContactDto? contactResponse = await _contactService.GetContactById(contact.ContactId);

            //Assert
            contactResponse.Should().BeEquivalentTo(contactDto);

        }

        #endregion

        #region GetAllContacts

        //The GetAllContacts() should return an empty list by default
        [Fact]
        public async Task GetAllContacts_ToBeEmptyList()
        {
            //Arrange
            var contacts = new List<Contact>();
            _contactRepositoryMock
              .Setup(temp => temp.GetAllContacts())
              .ReturnsAsync(contacts);

            //Act
            List<ContactDto> contacts_from_get = await _contactService.GetAllContacts();

            //Assert
            contacts_from_get.Should().BeEmpty();
        }


        //First, we will add few contacts; and then when we call GetAllContacts(), it should return the same contacts that were added
        [Fact]
        public async Task GetAllContacts_WithFewContacts_ToBeSuccessful()
        {
            //Arrange
            List<Contact> contacts = new List<Contact>()
            {
            _fixture.Build<Contact>()
                .With(temp => temp.ContactName, "test1")
                .With(temp => temp.UserID, 1)
                .With(temp => temp.PhoneNummber, "+381654")
                .With(temp => temp.User, null as User)
                .Create(),
            _fixture.Build<Contact>()
                .With(temp => temp.ContactName, "test2")
                .With(temp => temp.UserID, 1)
                .With(temp => temp.PhoneNummber, "+3816546")
                .With(temp => temp.User, null as User)
                .Create(),
            _fixture.Build<Contact>()
                .With(temp => temp.ContactName, "test3")
                .With(temp => temp.UserID, 1)
                .With(temp => temp.PhoneNummber, "+3816549")
                .With(temp => temp.User, null as User)
                .Create(),
            _fixture.Build<Contact>()
                .With(temp => temp.ContactName, "test13")
                .With(temp => temp.UserID, 2)
                .With(temp => temp.PhoneNummber, "+3816543")
                .With(temp => temp.User, null as User)
                .Create(),
            };

            List<ContactDto> contact_response_list_expected = _mapper.Map<List<ContactDto>>(contacts);

            _contactRepositoryMock.Setup(temp => temp.GetAllContacts()).ReturnsAsync(contacts);

            //Act
            List<ContactDto> contacts_list_from_get = await _contactService.GetAllContacts();

            //Assert
            contacts_list_from_get.Should().BeEquivalentTo(contact_response_list_expected);
        }
        #endregion

    }
}
