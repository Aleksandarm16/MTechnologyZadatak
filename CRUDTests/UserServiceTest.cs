using FluentAssertions;
using Moq;
using RepositoryContracts;
using Services;
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

        #endregion

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepository = _userRepositoryMock.Object;
            _userService = new UserService(_userRepository);

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

        #endregion
    }
}
