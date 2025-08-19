using AutoMapper;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Security.Cryptography;
using System.Text;

namespace FastxWebApi.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepoMock;

        private Mock<IRepository<int, Role>> _roleRepoMock;
        private Mock<IMapper> _mapperMock;

        private Mock<ILogger<UserService>> _loggerMock;
        private UserService _service;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _roleRepoMock = new Mock<IRepository<int, Role>>();

            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UserService>>();

            _service = new UserService(_userRepoMock.Object, _roleRepoMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        
        [Test]
        public async Task SoftDeleteUser_ValidId_ReturnsTrue()
        {
           
            _userRepoMock.Setup(repo => repo.SoftDelete(1)).ReturnsAsync(true);

           
            var result = await _service.SoftDeleteUser(1);

           
            Assert.IsTrue(result);
            _userRepoMock.Verify(repo => repo.SoftDelete(1), Times.Once);
        }

        
        [Test]
        public void SoftDeleteUser_InvalidId_ReturnsFalse()
        {
            
            _userRepoMock.Setup(repo => repo.SoftDelete(1)).ReturnsAsync(false);

            
            Assert.IsFalse(_service.SoftDeleteUser(1).Result);
            _userRepoMock.Verify(repo => repo.SoftDelete(1), Times.Once);
        }

        [Test]
        public async Task GetAllUsers_ReturnsPaginated_UserList()
        {
           
            var users = new List<User> { new User { UserId = 1, Role = new Role() } };
            var userDTOs = new List<UserDTO> { new UserDTO { UserId = 1 } };

            var pagination = new PagenationRequestDTO { PageNumber = 1, PageSize = 10 };
            var pagedResponse = new PagenationResponseDTO<User>
            {
                Items = users,TotalCount = users.Count,PageSize = pagination.PageSize,CurrentPage = pagination.PageNumber
            };

            _userRepoMock.Setup(repo => repo.GetAll(pagination)).ReturnsAsync(pagedResponse);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserDTO>>(users)).Returns(userDTOs);

           
            var result = await _service.GetAllUsers(pagination);

          
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(pagedResponse.TotalCount, result.TotalCount);
        }

        [Test]
        public void GetAllUsers_EmptyCollection_ThrowsNoEntriessInCollectionException()
        {
            
            var pagination = new PagenationRequestDTO { PageNumber = 1, PageSize = 10 };

            var emptyPagedResponse = new PagenationResponseDTO<User>
            {
                Items = new List<User>(),TotalCount = 0,PageSize = pagination.PageSize,CurrentPage = pagination.PageNumber
            };
            _userRepoMock.Setup(repo => repo.GetAll(pagination)).ReturnsAsync(emptyPagedResponse);

            
            Assert.ThrowsAsync<NoEntriessInCollectionException>(() => _service.GetAllUsers(pagination));
        }

        [Test]
        public async Task GetUserById_ValidId_ReturnsUserDTO()
        {
          
            var user = new User { UserId = 1, RoleId = 2, Role = new Role { RoleId = 2, RoleName = "Test" } };
            var userDto = new UserDTO { UserId = 1, RoleName = "Test" };

            _userRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(user);

            _mapperMock.Setup(m => m.Map<UserDTO>(user)).Returns(userDto);

            
            var result = await _service.GetUserById(1);

           
            Assert.IsNotNull(result);

            Assert.AreEqual(1, result.UserId);
            Assert.AreEqual("Test", result.RoleName);
        }

        [Test]
        public void GetUserById_InvalidId_ThrowsNoSuchEntityException()
        {
            
            _userRepoMock.Setup(r => r.GetById(1)).ReturnsAsync((User)null);

            
            Assert.ThrowsAsync<NoSuchEntityException>(() => _service.GetUserById(1));
        }

        
        [Test]
        public async Task RegisterUser_ValidInput_ReturnsUserDTO()
        {
           
            var registerRequest = new RegisterUserRequestDTO { Email = "newuser@example.com", Password = "newpassword123" };
            var userEntity = new User { UserId = 1, Email = registerRequest.Email };

            var userDto = new UserDTO { UserId = 1, Email = registerRequest.Email };

            _userRepoMock.Setup(r => r.GetUserByEmail(registerRequest.Email)).ReturnsAsync((User)null);
            _mapperMock.Setup(m => m.Map<User>(registerRequest)).Returns(userEntity);

            _userRepoMock.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(userEntity);
            _mapperMock.Setup(m => m.Map<UserDTO>(userEntity)).Returns(userDto);

            var result = await _service.RegisterUser(registerRequest);

            Assert.IsNotNull(result);
            Assert.AreEqual(userDto.UserId, result.UserId);
        }

        [Test]
        public void RegisterUser_EmailExists_ThrowsUserAlreadyExistsException()
        {
            
            var dto = new RegisterUserRequestDTO { Email = "exists@example.com" };

            var user = new User { UserId = 1 };

            _userRepoMock.Setup(repo => repo.GetUserByEmail(dto.Email)).ReturnsAsync(user);

            
            Assert.ThrowsAsync<UserAlreadyExistsException>(() => _service.RegisterUser(dto));
        }
        
        [Test]
        public async Task UpdateUserProfile_ValidId_ReturnsTrue()
        {
           
            var dto = new UpdateUserDTO { UserId = 1, Email = "updated@example.com" };

            var user = new User { UserId = 1, Email = "old@example.com" };

            _userRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map(dto, user));
            
            _userRepoMock.Setup(r => r.Update(user)).ReturnsAsync(user);

            
            var result = await _service.UpdateUserProfile(dto);

            
            Assert.IsTrue(result);
            _userRepoMock.Verify(r => r.Update(user), Times.Once);
        }
    }
}




