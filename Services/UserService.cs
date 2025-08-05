using AutoMapper;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace FastxWebApi.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<int, Role> _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository,IRepository<int, Role> roleRepository,IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> ChangeUserRole(int userId, int newRoleId)
        {
            try
            {
                
                var user = await _userRepository.GetById(userId);
                if (user == null)
                {
                    throw new NoSuchEntityException();
                }

                
                var role = await _roleRepository.GetById(newRoleId);
                if (role == null)
                {
                    throw new NoSuchEntityException();
                }

                
                user.RoleId = newRoleId;

                
                await _userRepository.Update(userId, user);

                return $"User {user.Name}'s role has been updated to {role.RoleName}.";
            }
            catch (NoSuchEntityException ex)
            {
                throw; 
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error changing user role.", ex);
            }
        }

        public async Task<string> DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete user with ID: {UserId}", id);

                var deletedUser = await _userRepository.Delete(id);
                if (deletedUser == null)
                {
                    _logger.LogWarning("Deletion failed: UserId {UserId} not found.", id);
                    throw new NoSuchEntityException();
                }
                _logger.LogInformation("User {UserId} deleted successfully.", id);

                return "User deleted Successfully";

            }
            catch(NoSuchEntityException ex)
            {
                throw new NoSuchEntityException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting user with ID: {UserId}", id);
                throw new Exception("Error deleting user.", ex);
            }

        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users.");

                var users = await _userRepository.GetAll();
                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No users found in the collection.");

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved all users.");

                //foreach (var user in users)
                //{
                //    user.Role = await _roleRepository.GetById(user.RoleId);
                //}
                return _mapper.Map<IEnumerable<UserDTO>>(users);
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all users.");

                throw new Exception("Error getting all users.");
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
                user.Role = await _roleRepository.GetById(user.RoleId);
                _logger.LogInformation("Successfully retrieved user with ID: {UserId}", id);

                return _mapper.Map<UserDTO>(user);
            }
            catch(NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user by ID: {UserId}", id);

                throw new Exception("Error getting user by id.");
            }
            

        }


        public async Task<string> RegisterUser(RegisterDTO dto)
        {
            try
            {

                _logger.LogInformation("Attempting to register user with email: {Email}", dto.Email);
                var existingUser = await _userRepository.GetUserByEmail(dto.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed: Email {Email} already exists.", dto.Email);
                    throw new Exception("User with this email already exists.");
                }

                var user = _mapper.Map<User>(dto);
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                user.Password = hashedPassword;

                var addedUser = await _userRepository.Add(user);
                if (addedUser != null)
                {
                    _logger.LogInformation("User {UserId} registered successfully.", addedUser.UserId);
                    return "User registered successfully.";
                }

                throw new Exception("User registration failed.");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user with email: {Email}", dto.Email);
                throw new Exception("Error registering user.");
            }
        }

        public async Task<string> UpdateUserProfile(UpdateUserDTO dto)
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
                await _userRepository.Update(user.UserId, user);
                _logger.LogInformation("User profile for UserId {UserId} updated successfully.", dto.UserId);

                return "User profile updated successfully.";
            }
            catch(NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user profile for UserId: {UserId}", dto.UserId);
                throw new Exception("Error updating user profile.");
            }
        }
    }
}
