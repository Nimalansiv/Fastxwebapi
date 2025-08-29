<<<<<<< HEAD:FastxWebApi-backend/AuthenticationService.cs
﻿using AutoMapper;
using FastxWebApi.Exceptions;
using System.Security.Cryptography;
using System.Text;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Models;
using FastxWebApi.Context;
using System.Security.Authentication;

namespace FastxWebApi.Services
{
    public class AuthenticationService:IAuthenticate
    {
        
        private readonly ITokenService _tokenService;
        public readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;
        public readonly ILogger<AuthenticationService> _logger;
        public AuthenticationService(IUserRepository UserRepository, ITokenService tokenService,IMapper mapper,ILogger<AuthenticationService>logger)
        {
            _userRepository = UserRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            _logger.LogInformation("Attempting to log in user with email: {Email}", loginRequest.Email);
            var dbUser = await _userRepository.GetUserByEmail(loginRequest.Email);

            if (dbUser == null)
            {
                _logger.LogWarning("Login failed, user with email {Email} not found.", loginRequest.Email);
                throw new InvalidCredentialsException("Invalid email or password.");
            }

            using var hmac = new HMACSHA256(dbUser.HashKey);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != dbUser.Password[i])
                {
                    _logger.LogWarning("Login failed, incorrect password for user {Email}.", loginRequest.Email);
                    throw new InvalidCredentialsException("Invalid email or password.");
                }
            }

            _logger.LogInformation("Login successful for user {Email}", loginRequest.Email);
            var token = await _tokenService.GenerateToken(new TokenUser
            {
                Username = dbUser.Name,
                Role = dbUser.Role.RoleName,
                UserId = dbUser.UserId,
            });

            return new LoginResponseDTO
            {
                Email = dbUser.Email,
                Name = dbUser.Name,
                UserId = dbUser.UserId,
                Role = dbUser.Role.RoleName,
                RoleId = dbUser.RoleId,
                Token = token
            };
        }

        public async Task<RegisterUserResponseDTO> RegisterUser(RegisterUserRequestDTO requestDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to register new user with email: {Email}", requestDTO.Email);
                var existingUser = await _userRepository.GetUserByEmail(requestDTO.Email);

                if (existingUser != null)
                {
                    _logger.LogInformation("Registration failed, user with email {Email} already exists.", requestDTO.Email);
                    throw new UserAlreadyExistsException("User with this email already exists.");
                }

                var newUser = _mapper.Map<User>(requestDTO);
                using var hmac = new HMACSHA256();
                newUser.HashKey = hmac.Key;
                newUser.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestDTO.Password));

                var addedUser = await _userRepository.Add(newUser);

                if (addedUser == null)
                {
                    _logger.LogError("User registration failed for unknown reason.");
                    throw new UserRegistrationException("User registration failed.");
                }

                _logger.LogInformation("User {UserId} registered successfully.", addedUser.UserId);
                return _mapper.Map<RegisterUserResponseDTO>(addedUser);
            }
            catch (UserAlreadyExistsException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during user registration.");
               
                throw new UserRegistrationException("Unable to add user.", ex);
            }
        }

    }
}
=======
﻿using AutoMapper;
using FastxWebApi.Exceptions;
using System.Security.Cryptography;
using System.Text;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Models;
using FastxWebApi.Context;
using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Services
{
    public class AuthenticationService:IAuthenticate
    {
        
        private readonly ITokenService _tokenService;
        public readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;
        public readonly ILogger<AuthenticationService> _logger;
        private readonly ApplicationDbContext _context;
        public AuthenticationService(IUserRepository UserRepository, ITokenService tokenService,IMapper mapper,ILogger<AuthenticationService>logger, ApplicationDbContext context)
        {
            _userRepository = UserRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            _logger.LogInformation("Attempting to log in user with email: {Email}", loginRequest.Email);
            var dbUser = await _userRepository.GetUserByEmail(loginRequest.Email);

            if (dbUser == null)
            {
                _logger.LogWarning("Login failed, user with email {Email} not found.", loginRequest.Email);
                throw new InvalidCredentialsException("Invalid email or password.");
            }

            using var hmac = new HMACSHA256(dbUser.HashKey);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != dbUser.Password[i])
                {
                    _logger.LogWarning("Login failed, incorrect password for user {Email}.", loginRequest.Email);
                    throw new InvalidCredentialsException("Invalid email or password.");
                }
            }

            _logger.LogInformation("Login successful for user {Email}", loginRequest.Email);
            var token = await _tokenService.GenerateToken(new TokenUser
            {
                Username = dbUser.Name,
                Role = dbUser.Role.RoleName,
                UserId = dbUser.UserId,
            });

            return new LoginResponseDTO
            {
                Email = dbUser.Email,
                Name = dbUser.Name,
                UserId = dbUser.UserId,
                Role = dbUser.Role.RoleName,
                RoleId = dbUser.RoleId,
                Token = token
            };
        }

        public async Task<RegisterUserResponseDTO> RegisterUser(RegisterUserRequestDTO requestDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to register new user with email: {Email}", requestDTO.Email);
                var existingUser = await _userRepository.GetUserByEmail(requestDTO.Email);

                if (existingUser != null)
                {
                    _logger.LogInformation("Registration failed, user with email {Email} already exists.", requestDTO.Email);
                    throw new UserAlreadyExistsException("User with this email already exists.");
                }

                var newUser = _mapper.Map<User>(requestDTO);
                using var hmac = new HMACSHA256();
                newUser.HashKey = hmac.Key;
                newUser.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestDTO.Password));

                var addedUser = await _userRepository.Add(newUser);

                if (addedUser == null)
                {
                    _logger.LogError("User registration failed for unknown reason.");
                    throw new UserRegistrationException("User registration failed.");
                }

                _logger.LogInformation("User {UserId} registered successfully.", addedUser.UserId);
                return _mapper.Map<RegisterUserResponseDTO>(addedUser);
            }
            catch (UserAlreadyExistsException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during user registration.");
               
                throw new UserRegistrationException("Unable to add user.", ex);
            }
        }

        public async Task<string> ForgotPassword(ForgotPasswordDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return "user not found";

            using var hmac = new HMACSHA256(user.HashKey);  // <- use the stored key
            user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.NewPassword));

            await _context.SaveChangesAsync();
            return "Password reset successfully";
        }

        public async Task<string> ChangePassword(ChangePasswordDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.UserId);
            if (user == null)
                return "user not found";

            using var hmac = new HMACSHA256(user.HashKey); // <- use the stored key
            var currentPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.CurrentPassword));

            if (!user.Password.SequenceEqual(currentPasswordHash))
                return "Current password is incorrect";

            user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.NewPassword));
            await _context.SaveChangesAsync();
            return "Password changed successfully";
        }


        private byte[] CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA256();
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Services/AuthenticationService.cs
