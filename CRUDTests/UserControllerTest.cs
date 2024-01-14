using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServicesContracts;
using ServicesContracts.DTO;
using Xunit;
using ZadatakApi.Controllers;

namespace CRUDTests
{
    public class UserControllerTest
    {
        //private fields
        private readonly IUserService _userService;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Fixture _fixture;

        public UserControllerTest()
        {
            _fixture = new Fixture();
            _userServiceMock = new Mock<IUserService>();
            _userService = _userServiceMock.Object;
        }

        #region Create
        //If the UserDto has some missing information bad request should be thrown
        [Fact]
        public async void CreateUser_IfModelErrors_ToReturnBadRequest()
        {
            //Arrange
            UserDto user_add_request = _fixture.Create<UserDto>();
            UserDto user_response = _fixture.Create<UserDto>();

            _userServiceMock
                .Setup(temp => temp.AddUser(It.IsAny<UserDto>()))
                .ReturnsAsync(user_response);

            UserController userController = new UserController(_userService);

            //Act
            userController.ModelState.AddModelError("PhoneNummber", "User phone nummber can't be blank");

            IActionResult result = await userController.CreateUser(user_add_request);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        //If we supply valid UserDto object we should get Ok object result from controller
        [Fact]
        public async void CreateUser_ShouldReturnOkRequest()
        {
            //Arrange
            UserDto user_add_request = _fixture.Create<UserDto>();
            UserDto user_response = _fixture.Create<UserDto>();

            _userServiceMock
                .Setup(temp => temp.AddUser(It.IsAny<UserDto>()))
                .ReturnsAsync(user_response);

            UserController userController = new UserController(_userService);

            //Act
            IActionResult result = await userController.CreateUser(user_add_request);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region GetAvailableContacts

        //If we supply invalid ID we should get BadRequest
        [Fact]
        public async Task GetUserContact_UserIdNull_ShouldThrowBadRequest()
        {
            //Arrange
            int? userId = null;   
            
            UserController userController = new UserController(_userService);

            //Act

            IActionResult result = await userController.GetAvailableContacts(userId);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        //If we can not find user in the database it should throw NotFoundResult
        [Fact]
        public async Task GetUserContact_UserIdInvalid_ShouldThrowNotFoundResult()
        {
            //Arrange
            int? userId = 5;
            UserDto? user_response = null;

            UserController userController = new UserController(_userService);
            _userServiceMock
               .Setup(temp => temp.GetUserById(userId))
               .ReturnsAsync(user_response);

            //Act

            IActionResult result = await userController.GetAvailableContacts(userId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        //If we supply valid UserDto it should return Ok result and Empty List because we didn't find any contacts
        [Fact]
        public async Task GetUserContact_UserIdValid_ShouldReturnOK()
        {
            //Arrange
            int? userId = 5;
            UserDto? user_response = _fixture.Create<UserDto>();
            List<UserDto> user_responseList = new List<UserDto>();

            UserController userController = new UserController(_userService);
            _userServiceMock
               .Setup(temp => temp.GetUserById(userId))
               .ReturnsAsync(user_response);

            _userServiceMock
                .Setup(temp => temp.GetAllAvaiableContacts(user_response))
                .ReturnsAsync(user_responseList);

            //Act

            var result = await userController.GetAvailableContacts(userId);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualUsers = Assert.IsAssignableFrom<List<UserDto>>(okResult.Value);

            actualUsers.Should().BeEmpty();       
        }

        //If we supply valid UserDto it should return Ok result and contacts for that user 
        [Fact]
        public async Task GetUserContact_UserIdValid_ShouldReturnOk_And_ListOfUsers()
        {
            //Arrange
            int? userId = 5;
            UserDto? user_response = _fixture.Create<UserDto>();
            List<UserDto> user_responseList = new List<UserDto>()
            { 
                _fixture.Create<UserDto>(),
                _fixture.Create<UserDto>(),
                _fixture.Create<UserDto>()
            };

            UserController userController = new UserController(_userService);
            _userServiceMock
               .Setup(temp => temp.GetUserById(userId))
               .ReturnsAsync(user_response);

            _userServiceMock
                .Setup(temp => temp.GetAllAvaiableContacts(user_response))
                .ReturnsAsync(user_responseList);

            //Act

            var result = await userController.GetAvailableContacts(userId);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualUsers = Assert.IsAssignableFrom<List<UserDto>>(okResult.Value);

            actualUsers.Should().BeEquivalentTo(user_responseList);

        }


        #endregion

        #region Delete

        //If we supply invalid Id should return Bad Request
        [Fact]
        public async Task DeleteUser_InvalidID()
        {
            //Arrange
            int? userId = null;

            UserController userController = new UserController(_userService);

            //Act
            IActionResult result = await userController.DeleteUser(userId);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }
        //If we supply valid Id and user was not found it should return NotFound
        [Fact]
        public async Task DeleteUser_ValidID_ReturnsNotFound()
        {
            //Arrange
            int? userId = 3;
            UserDto? user_response = null;

            UserController userController = new UserController(_userService);
            _userServiceMock
                .Setup(temp => temp.GetUserById(userId))
                .ReturnsAsync(user_response);
            _userServiceMock
              .Setup(temp => temp.DeleteUser(userId))
              .ReturnsAsync(false);

            //Act
            IActionResult result = await userController.DeleteUser(userId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        //If we supply valid Id and user was found it should return Ok
        [Fact]
        public async Task DeleteUser_ValidID_ReturnsOk()
        {
            //Arrange
            int? userId = 3;
            UserDto? user_response = _fixture.Create<UserDto>();

            UserController userController = new UserController(_userService);
            _userServiceMock
                .Setup(temp => temp.GetUserById(userId))
                .ReturnsAsync(user_response);

            _userServiceMock
                .Setup(temp => temp.DeleteUser(userId))
                .ReturnsAsync(true);
            //Act
            IActionResult result = await userController.DeleteUser(userId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region EditUser

        //If we supply invalid UserID it should return badrequest object

        [Fact]
        public async Task EditUser_InvalidId_ToReturn_BadRequestObject()
        {
            //Arrange
            UserDto? user = null;
            UserController userController = new UserController(_userService);

            //Act
            IActionResult result = await userController.EditUser(user);
            
            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        //If the model state is valid it should update the user
        [Fact]
        public async Task EditUser_ValidId_ToReturn_OkObjectResult()
        {
            UserDto? user_post = _fixture.Create<UserDto>();
            UserController userController = new UserController(_userService);
            _userServiceMock
                .Setup(temp => temp.GetUserById(user_post.UserID))
                .ReturnsAsync(user_post);
            _userServiceMock
                .Setup(temp => temp.UpdateUser(user_post))
                .ReturnsAsync(user_post);

            //Act
            IActionResult result = await userController.EditUser(user_post);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        //If the model state is not valid it should return Bad request Object
        [Fact]
        public async Task EditUser_ValidId_ToReturn_BadRequistObject()
        {
            UserDto? user_post = _fixture.Create<UserDto>();
            UserController userController = new UserController(_userService);

            //Act
            userController.ModelState.AddModelError("PhoneNummber", "User phone nummber can't be blank");
            IActionResult result = await userController.EditUser(user_post);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion
    }
}
