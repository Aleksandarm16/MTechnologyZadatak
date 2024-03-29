﻿using AutoFixture;
using AutoMapper;
using Entities;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using Services;
using Services.EntityProfiler;
using ServicesContracts;
using ServicesContracts.DTO;
using System.Diagnostics.Metrics;
using Xunit;

namespace CRUDTests
{
    public class UserServiceTest
    {
        #region Private Fields

        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IFixture _fixture;

        #endregion

        public UserServiceTest()
        {
            _fixture = new Fixture();
            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepository = _userRepositoryMock.Object;
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(AppMappingProfile));
            });
            var mapper = mapperConfiguration.CreateMapper();
            _mapper = mapper;

            _userService = new UserService(_userRepository, _mapper);
        }

        #region AddUser
        //If the user is invalid it should throw argument null exception
        [Fact]
        public async Task AddUser_Nulluser_ToBeArgumentNullException()
        {
            //Arrange
            UserDto? userDto = null;

            //Act
            Func<Task> action = async () =>
            {
                await _userService.AddUser(userDto);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddUser_UserPhoneIsNull_ToBeArgumentException()
        {
            //Arrange

            UserDto? userDto = new UserDto { Email = "ajshdj", UserName = "test23" };
            User user = _mapper.Map<User>(userDto);
            //When UserRepository.AddUser is called, it has to return the same user object
            _userRepositoryMock.Setup(temp => temp.AddUser(It.IsAny<User>()))
                .ReturnsAsync(user);

            //Act
            Func<Task> action = async () =>
            {
                await _userService.AddUser(userDto);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddUser_FullUserDetails_ToBeSuccessful()
        {
            //Arrange
            UserDto userDto = new UserDto { Email = "test1@gmail.com", PhoneNummber = "+38123676", UserName = "Test12" };

            User user = _mapper.Map<User>(userDto);
            UserDto expectedUser = _mapper.Map<UserDto>(user);

            _userRepositoryMock.Setup(temp => temp.AddUser(It.IsAny<User>()))
                .ReturnsAsync(user);

            //Act

            UserDto userDtoResponse = await _userService.AddUser(userDto);

            //Assert
            userDtoResponse.Should().Be(expectedUser);
        }

        #endregion

        #region Update User

        //When we supply null as User object it should throw Argument Null Exception
        [Fact]
        public async Task UpdateUser_NullUser_ToBeArgumentNullException()
        {
            //Arrange
            UserDto? userDto = null;

            //Act
            Func<Task> action = async () =>
            {
                await _userService.UpdateUser(userDto);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply invalid user id, it should throw Argument Exception
        [Fact]
        public async Task UpdateUser_InvalidUserId_ToBeArgumentException()
        {
            //Arrange
            UserDto? userDto = _fixture.Build<UserDto>()
                .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _userService.UpdateUser(userDto);
            };

            //Assert 
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //First, add a new user and try to update the user name and email

        [Fact]
        public async Task UpdateUser_UserFullDetails_ToBeSuccessfull()
        {
            //Arrange
            UserDto userDto = _fixture.Build<UserDto>()
                .With(x => x.Email, "test12@gmail.com")
                .With(x => x.PhoneNummber, "+381652323")
                .With(x => x.UserName, "testuser")
                .With(x => x.UserID, 2)
                .Create();

            User user = _mapper.Map<User>(userDto);

            _userRepositoryMock.Setup(x => x.GetUserByUserId(userDto.UserID))
                .ReturnsAsync(user);
            _userRepositoryMock
                .Setup(temp => temp.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(user);

            //Act
            UserDto responseUser = await _userService.UpdateUser(userDto);

            //Assert
            responseUser.Should().Be(userDto);
        }

        #endregion

        #region GetUserByUserID
        //If we supplu null as userId it should retun null as response
        [Fact]
        public async Task GetUserByUserId_NullUserId_ToBeNull()
        {
            //Arrange
            int? userId = null;

            //Act
            UserDto? userDto = await _userService.GetUserById(userId);

            //Assert
            userDto.Should().BeNull();
        }

        //If we supply a valid user id, it should return the valid user details as UserDto object
        [Fact]
        public async Task GetUserByUserId_WithUserId_ToBeSuccessful()
        {
            //Arange
            User user = _fixture.Build<User>()
                .With(x => x.UserID, 2)
                .With(x => x.UserName, "test1")
                .With(x => x.PhoneNummber, "+38167233")
                .With(x => x.Email, "test@gmail.com")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create();

            UserDto userDto = _mapper.Map<UserDto>(user);

            _userRepositoryMock.Setup(x => x.GetUserByUserId(user.UserID))
                .ReturnsAsync(user);

            //Act
            UserDto? userResponse = await _userService.GetUserById(user.UserID);

            //Assert
            userResponse.Should().Be(userDto);

        }

        #endregion

        #region DeleteUser

        //If you supply an valid UserId, it should return true
        [Fact]
        public async Task DeleteUser_ValidUserId_ToBeSuccessful()
        {
            //Arrange
            User user = _fixture.Build<User>()
                .With(x => x.UserID, 2)
                .With(x => x.UserName, "test1")
                .With(x => x.PhoneNummber, "+38167233")
                .With(x => x.Email, "test@gmail.com")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create();

            _userRepositoryMock.Setup(x => x.DeleteUser(It.IsAny<int>()))
                .ReturnsAsync(true);

            _userRepositoryMock
                .Setup(x => x.GetUserByUserId(It.IsAny<int>()))
                .ReturnsAsync(user);

            //Act
            bool isDeleted = await _userService.DeleteUser(user.UserID);

            //Assert
            isDeleted.Should().BeTrue();
        }

        // If you supply an invalid UserId it should return false
        [Fact]
        public async Task DeleteUser_InvalidUserId()
        {
            //Arrange
            User user = _fixture.Build<User>()
                .With(x => x.UserID, 2)
                .With(x => x.UserName, "test1")
                .With(x => x.PhoneNummber, "+38167233")
                .With(x => x.Email, "test@gmail.com")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create();

            _userRepositoryMock.Setup(x => x.DeleteUser(1))
                .ReturnsAsync(true);

            _userRepositoryMock
                .Setup(x => x.GetUserByUserId(1))
                .ReturnsAsync(user);
            //Act
            bool isDeleted = await _userService.DeleteUser(user.UserID);

            //Assert
            isDeleted.Should().BeFalse();
        }

        #endregion

        #region GetContactsForUser

        //By defaullt GetAllAvailableUsers should return an empty list
        [Fact]
        public async Task GetUserContact_NoContactsFound()
        {
            //Arrange
            User user = _fixture.Build<User>()
                .With(temp => temp.UserName, "Marko")
                .With(temp => temp.UserID, 1)
                .With(temp => temp.PhoneNummber, "+3816999")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create();

            UserDto user_for_send = _mapper.Map<UserDto>(user);

            var usersList = new List<User>();
            _userRepositoryMock
                .Setup(x => x.GetAllAvaiableContacts(null))
                .ReturnsAsync(usersList);

            _userRepositoryMock.Setup(temp => temp.GetUserByUserId(user.UserID)).ReturnsAsync(user);
            //Act
            List<UserDto> users_from_get = await _userService.GetAllAvaiableContacts(user_for_send);

            //Assert
            users_from_get.Should().BeEmpty();


        }

        //If userId is not valid it should throw ArgumentNullException
        [Fact]
        public async Task GetUserContact_NullUser_ToBeArgumentNullException()
        {
            //Arrange
            UserDto? userDto = null;

            //Act
            Func<Task> action = async () =>
            {
                await _userService.GetAllAvaiableContacts(userDto);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //First we add few contacts and then when we call GetUSerContact() it should return the same Contacts that were added
        [Fact]
        public async Task GetuserContacts_WithFewContacts_ToBeSuccessful()

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

            List<User> users = new List<User>()
            {
            _fixture.Build<User>()
                .With(temp => temp.UserName, "Marko")
                .With(temp => temp.UserID, 1)
                .With(temp => temp.PhoneNummber, "+3816999")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create(),
            _fixture.Build<User>()
                .With(temp => temp.UserName, "Nikola")
                .With(temp => temp.UserID, 1)
                .With(temp => temp.PhoneNummber, "+3816546")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create(),
            _fixture.Build<User>()
                .With(temp => temp.UserName, "Pera")
                .With(temp => temp.UserID, 1)
                .With(temp => temp.PhoneNummber, "+3816549")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create(),
             _fixture.Build<User>()
                .With(temp => temp.UserName, "Sima")
                .With(temp => temp.UserID, 2)
                .With(temp => temp.PhoneNummber, "+381654932")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create()
            };

            List<User> excpected_entity =  users.Take(users.Count - 1).ToList();

            List<UserDto> excpected_result = _mapper.Map<List<UserDto>>(excpected_entity);

            _userRepositoryMock.Setup(temp => temp.GetAllAvaiableContacts(users.First())).ReturnsAsync(excpected_entity);
            _userRepositoryMock.Setup(temp => temp.GetUserByUserId(users.First().UserID)).ReturnsAsync(users.First());

            UserDto user = _mapper.Map<UserDto>(users.First());

            //Act
            List<UserDto> users_from_get = await _userService.GetAllAvaiableContacts(user);

            //Assert
            users_from_get.Should().BeEquivalentTo(excpected_result);

        }
        #endregion

        #region GetAllUsers

        //The GetAllUsers() should return an empty list by default
        [Fact]
        public async Task GetAllUsers_ToBeEmptyList()
        {
            //Arrange
            var users = new List<User>();
            _userRepositoryMock
              .Setup(temp => temp.GetAllUsers())
              .ReturnsAsync(users);

            //Act
            List<UserDto> users_from_get = await _userService.GetAllUsers();

            //Assert
            users_from_get.Should().BeEmpty();
        }


        //First, we will add few users; and then when we call GetAllUsers(), it should return the same users that were added
        [Fact]
        public async Task GetAllUsers_WithFewUsers_ToBeSuccessful()
        {
            //Arrange
            List<User> users = new List<User>()
            {
            _fixture.Build<User>()
                .With(temp => temp.UserName, "Marko")
                .With(temp => temp.UserID, 1)
                .With(temp => temp.PhoneNummber, "+3816999")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create(),
            _fixture.Build<User>()
                .With(temp => temp.UserName, "Nikola")
                .With(temp => temp.UserID, 1)
                .With(temp => temp.PhoneNummber, "+3816546")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create(),
            _fixture.Build<User>()
                .With(temp => temp.UserName, "Pera")
                .With(temp => temp.UserID, 1)
                .With(temp => temp.PhoneNummber, "+3816549")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create(),
             _fixture.Build<User>()
                .With(temp => temp.UserName, "Sima")
                .With(temp => temp.UserID, 2)
                .With(temp => temp.PhoneNummber, "+381654932")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create()
            };

            List<UserDto> user_response_list_expected = _mapper.Map<List<UserDto>>(users);           

            _userRepositoryMock.Setup(temp => temp.GetAllUsers()).ReturnsAsync(users);

            //Act
            List<UserDto> users_list_from_get = await _userService.GetAllUsers();

            //Assert
            users_list_from_get.Should().BeEquivalentTo(user_response_list_expected);
        }
        #endregion


        #region SendMessage

        //Send Message invalid message throws Argument Null Exception

        [Fact]
        public async Task SendMessage_InvalidMessage_ThrowsArgumentNull()
        {
            //Arrange           

            MessageDto? message = null;

            //Act

            Func<Task> action = async () =>
            {
                await _userService.SendMessageToContats(message);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //If we supply invalid Recipient or it isn't in found with GetAvailableContacts() it should return false
        [Fact]
        public async Task SendMessage_ContactNotFound_ReturnsFalse()
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
                .With(temp => temp.ContactName, "Nikola")
                .With(temp => temp.UserID, 2)
                .With(temp => temp.PhoneNummber, "+3816546")
                .With(temp => temp.User, null as User)
                .Create(),
            _fixture.Build<Contact>()
                .With(temp => temp.ContactName, "test3")
                .With(temp => temp.UserID, 3)
                .With(temp => temp.PhoneNummber, "+3816549")
                .With(temp => temp.User, null as User)
                .Create(),
            _fixture.Build<Contact>()
                .With(temp => temp.ContactName, "test13")
                .With(temp => temp.UserID, 4)
                .With(temp => temp.PhoneNummber, "+3816543")
                .With(temp => temp.User, null as User)
                .Create(),
            };

            List<User> users = new List<User>()
            {
            _fixture.Build<User>()
                .With(temp => temp.UserName, "Marko")
                .With(temp => temp.UserID, 1)
                .With(temp => temp.PhoneNummber, "+3816999")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create(),
            _fixture.Build<User>()
                .With(temp => temp.UserName, "Nikola")
                .With(temp => temp.UserID, 2)
                .With(temp => temp.PhoneNummber, "+3816546")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create(),
            _fixture.Build<User>()
                .With(temp => temp.UserName, "Pera")
                .With(temp => temp.UserID, 3)
                .With(temp => temp.PhoneNummber, "+3816549")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create(),
             _fixture.Build<User>()
                .With(temp => temp.UserName, "Sima")
                .With(temp => temp.UserID, 4)
                .With(temp => temp.PhoneNummber, "+381654932")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create()
            };
            MessageDto message = _fixture.Build<MessageDto>()
                .With(temp => temp.Message,"test message")
                .With(temp => temp.Sender, "Marko")
                .With(temp => temp.SenderId,1)
                .With(temp => temp.Recipient, "Nikola")
                .With(temp => temp.RecipientId,2)
                .With(temp => temp.Id,1)
                .Create();

            _userRepositoryMock.Setup(temp => temp.GetUserByUserId(users.First().UserID)).ReturnsAsync(users.First());
            _userRepositoryMock.Setup(temp => temp.GetUserByUserId(users[1].UserID)).ReturnsAsync(users[1]);
            _userRepositoryMock.Setup(temp => temp.GetAllAvaiableContacts(users.First())).ReturnsAsync(new List<User>());

            //Act

            bool result = await _userService.SendMessageToContats(message);

            //Assert

            result.Should().BeFalse();  
        }

        //If we supply valid Recipient SendMessageToContact() should return true
        [Fact]
        public async Task SendMessage_ToBeSuccessful()
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
                .With(temp => temp.ContactName, "Nikola")
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

            List<User> users = new List<User>()
            {
            _fixture.Build<User>()
                .With(temp => temp.UserName, "Marko")
                .With(temp => temp.UserID, 1)
                .With(temp => temp.PhoneNummber, "+3816999")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create(),
            _fixture.Build<User>()
                .With(temp => temp.UserName, "Nikola")
                .With(temp => temp.UserID, 2)
                .With(temp => temp.PhoneNummber, "+3816546")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create(),
            _fixture.Build<User>()
                .With(temp => temp.UserName, "Pera")
                .With(temp => temp.UserID, 3)
                .With(temp => temp.PhoneNummber, "+3816549")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create(),
             _fixture.Build<User>()
                .With(temp => temp.UserName, "Sima")
                .With(temp => temp.UserID, 4)
                .With(temp => temp.PhoneNummber, "+381654932")
                .With(temp => temp.Contacts, null as ICollection<Contact>)
                .Create()
            };
            MessageDto message = _fixture.Build<MessageDto>()
                .With(temp => temp.Message, "test message")
                .With(temp => temp.Sender, "Marko")
                .With(temp => temp.SenderId, 1)
                .With(temp => temp.Recipient, "Nikola")
                .With(temp => temp.RecipientId,2)
                .With(temp => temp.Id, 1)
                .Create();

            List<User> excpected_entity = users.Skip(1).Take(1).ToList();

            _userRepositoryMock.Setup(temp => temp.GetUserByUserId(users.First().UserID)).ReturnsAsync(users.First());
            _userRepositoryMock.Setup(temp => temp.GetUserByUserId(users[1].UserID)).ReturnsAsync(users[1]);
            _userRepositoryMock.Setup(temp => temp.GetAllAvaiableContacts(users.First())).ReturnsAsync(excpected_entity);


            //Act

            bool result = await _userService.SendMessageToContats(message);

            //Assert

            result.Should().BeTrue();
        }

        #endregion
    }
}
