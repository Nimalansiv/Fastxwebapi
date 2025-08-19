using AutoMapper;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace FastxWebApi.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<int, Role> _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IRepository<int, Role> roleRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> SoftDeleteUser(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to soft delete user with ID: {UserId}", id);
                var softDeleteResult = await _userRepository.SoftDelete(id);
                if (!softDeleteResult)
                {
                    _logger.LogWarning("Soft delete failed: UserId {UserId} not found or already deleted.", id);
                    return false;
                }
                _logger.LogInformation("User {UserId} soft deleted successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while soft deleting user with ID: {UserId}", id);
                throw new Exception("Error soft deleting user.", ex);
            }
        }

        public async Task<PagenationResponseDTO<UserDTO>> GetAllUsers(PagenationRequestDTO pagenation)
        {
            try
            {
                _logger.LogInformation("Fetching all users with pagination.");
                var usersPage = await _userRepository.GetAll(pagenation);
                if (usersPage == null || !usersPage.Items.Any())
                {
                    _logger.LogWarning("No users found in the collection.");
                    throw new NoEntriessInCollectionException("No users found.");
                }
                var userDtos = _mapper.Map<IEnumerable<UserDTO>>(usersPage.Items);
                return new PagenationResponseDTO<UserDTO>
                {
                    Items = userDtos,
                    TotalCount = usersPage.TotalCount,
                    PageSize = usersPage.PageSize,
                    CurrentPage = usersPage.CurrentPage
                };
            }
            catch (NoEntriessInCollectionException) 
            { 
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all users.");
                throw new Exception("Error getting all users.", ex);
            }
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user by ID: {UserId}", id);
                var user = await _userRepository.GetById(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", id);
                    throw new NoSuchEntityException();
                }
                _logger.LogInformation("Successfully retrieved user with ID: {UserId}", id);
                return _mapper.Map<UserDTO>(user);
            }
            catch (NoSuchEntityException) 
            { 
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user by ID: {UserId}", id);
                throw new Exception("Error getting user by id.", ex);
            }
        }

        public async Task<UserDTO> RegisterUser(RegisterUserRequestDTO dto)
        {
            try
            {
                _logger.LogInformation("Attempting to register user with email: {Email}", dto.Email);
                var existingUser = await _userRepository.GetUserByEmail(dto.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed: Email {Email} already exists.", dto.Email);
                    throw new UserAlreadyExistsException("User with this email already exists.");
                }

                var user = _mapper.Map<User>(dto);
                using var hmac = new HMACSHA256();
                user.HashKey = hmac.Key;
                user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

                var addedUser = await _userRepository.Add(user);
                if (addedUser != null)
                {
                    _logger.LogInformation("User {UserId} registered successfully.", addedUser.UserId);
                    return _mapper.Map<UserDTO>(addedUser);
                }

                throw new Exception("User registration failed.");
            }
            catch (UserAlreadyExistsException) 
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user with email: {Email}", dto.Email);
                throw new Exception("Error registering user.", ex);
            }
        }

        public async Task<bool> UpdateUserProfile(UpdateUserDTO dto)
        {
            try
            {
                _logger.LogInformation("Attempting to update user profile for UserId: {UserId}", dto.UserId);
                var user = await _userRepository.GetById(dto.UserId);
                if (user == null)
                {
                    _logger.LogWarning("Update failed: UserId {UserId} not found.", dto.UserId);
                    throw new NoSuchEntityException();
                }

                _mapper.Map(dto, user);
                await _userRepository.Update(user);
                _logger.LogInformation("User profile for UserId {UserId} updated successfully.", dto.UserId);
                return true;
            }
            catch (NoSuchEntityException)
            { 
                throw; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user profile for UserId: {UserId}", dto.UserId);
                throw new Exception("Error updating user profile.", ex);
            }
        }
    }
}
