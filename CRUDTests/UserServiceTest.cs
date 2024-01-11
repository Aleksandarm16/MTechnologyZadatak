using AutoMapper;
using Entities;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using Services;
using Services.EntityProfiler;
using ServicesContracts;
using ServicesContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #endregion

        public UserServiceTest()
        {
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

            UserDto? userDto = new UserDto { Email = "ajshdj", UserName = "test23"};
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
    }
}
