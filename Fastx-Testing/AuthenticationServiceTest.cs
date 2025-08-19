using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FastxWebApi.Interfaces;
using Microsoft.Extensions.Logging;
using FastxWebApi.Services;
using Moq;
using FastxWebApi.Models;
using System.Security.Cryptography;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Exceptions;

namespace FastxTest
{
    internal class AuthenticationServiceTest
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<ITokenService> _mockTokenService;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<AuthenticationService>> _mockLogger;
        private AuthenticationService _authenticationService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTokenService = new Mock<ITokenService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<AuthenticationService>>();

            _authenticationService = new AuthenticationService(
                _mockUserRepository.Object,
                _mockTokenService.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }
        private User CreateUserWithHashedPassword(string email, string plainTextPassword, string roleName)
        {
            using var hmac = new HMACSHA256();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword));
            var hashKey = hmac.Key;

            return new User
            {
                UserId = 1,Email = email,Name = "TestUser",Password = passwordHash,HashKey = hashKey,
                Role = new Role { RoleName = roleName },RoleId = 1
            };
        }
        [Test]
        public async Task Login_WithCorrectCredentials_ReturnsLoginResponseDTO()
        {
            
            var loginRequest = new LoginRequestDTO { Email = "test@example.com", Password = "password123" };
            var dbUser = CreateUserWithHashedPassword(loginRequest.Email, loginRequest.Password, "User");

            _mockUserRepository.Setup(r => r.GetUserByEmail(loginRequest.Email)).ReturnsAsync(dbUser);
            _mockTokenService.Setup(s => s.GenerateToken(It.IsAny<TokenUser>())).ReturnsAsync("fake_token");

            var result = await _authenticationService.Login(loginRequest);
            Assert.IsNotNull(result);
            Assert.AreEqual(dbUser.Email, result.Email);
            Assert.AreEqual("fake_token", result.Token);
            _mockUserRepository.Verify(r => r.GetUserByEmail(loginRequest.Email), Times.Once);
            _mockTokenService.Verify(s => s.GenerateToken(It.IsAny<TokenUser>()), Times.Once);
        }
        [Test]
        public void Login_WithIncorrectPassword_ThrowsInvalidCredentialsException()
        {
           
            var loginRequest = new LoginRequestDTO { Email = "test@example.com", Password = "wrongpassword" };
            var dbUser = CreateUserWithHashedPassword(loginRequest.Email, "correctpassword", "User");

            _mockUserRepository.Setup(r => r.GetUserByEmail(loginRequest.Email)).ReturnsAsync(dbUser);
            Assert.ThrowsAsync<InvalidCredentialsException>(async () => await _authenticationService.Login(loginRequest));
        }

        [Test]
        public void Login_WhenUserNotFound_ThrowsInvalidCredentialsException()
        {
            
            var loginRequest = new LoginRequestDTO { Email = "nonexistent@example.com", Password = "password123" };
            _mockUserRepository.Setup(r => r.GetUserByEmail(loginRequest.Email)).ReturnsAsync((User)null);

            
            Assert.ThrowsAsync<InvalidCredentialsException>(async () => await _authenticationService.Login(loginRequest));
        }
        [Test]
        public async Task RegisterUser_WithUniqueEmail_ReturnsRegisterUserResponseDTO()
        {
            
            var registerRequest = new RegisterUserRequestDTO { Email = "newuser@example.com", Password = "newpassword123", Name = "New User" };
            var newUser = new User { Email = registerRequest.Email };
            var addedUser = new User { UserId = 2, Email = registerRequest.Email, Name = registerRequest.Name };

            var responseDTO = new RegisterUserResponseDTO { UserId = addedUser.UserId };

            _mockUserRepository.Setup(r => r.GetUserByEmail(registerRequest.Email)).ReturnsAsync((User)null);
            _mockMapper.Setup(m => m.Map<User>(registerRequest)).Returns(newUser);
            _mockUserRepository.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(addedUser);
            _mockMapper.Setup(m => m.Map<RegisterUserResponseDTO>(addedUser)).Returns(responseDTO);

           
            var result = await _authenticationService.RegisterUser(registerRequest);

            
            Assert.IsNotNull(result);
            Assert.AreEqual(addedUser.UserId, result.UserId);
            _mockUserRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        }
        [Test]
        public void RegisterUser_WhenUserAlreadyExists_ThrowsUserAlreadyExistsException()
        {
           
            var registerRequest = new RegisterUserRequestDTO { Email = "existing@example.com", Password = "password123" };
            var existingUser = new User { Email = registerRequest.Email };

            
            _mockUserRepository.Setup(r => r.GetUserByEmail(registerRequest.Email)).ReturnsAsync(existingUser);

            
            Assert.ThrowsAsync<UserAlreadyExistsException>(async () => await _authenticationService.RegisterUser(registerRequest));
            _mockUserRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
        }

        [Test]
        public void RegisterUser_WhenAddFails_ThrowsUserRegistrationException()
        {
           
            var registerRequest = new RegisterUserRequestDTO { Email = "newuser@example.com", Password = "newpassword123", Name = "New User" };
            var user = new User { Email = registerRequest.Email };

           
            _mockUserRepository.Setup(r => r.GetUserByEmail(registerRequest.Email)).ReturnsAsync((User)null);
            _mockMapper.Setup(m => m.Map<User>(registerRequest)).Returns(user);
            _mockUserRepository.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync((User)null);

            
            Assert.ThrowsAsync<UserRegistrationException>(async () => await _authenticationService.RegisterUser(registerRequest));
            _mockUserRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        }

    }
}
