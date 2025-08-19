using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastxWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("DefaultCORS")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            try
            {
                var result = await _userService.SoftDeleteUser(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound($"User with ID {id} not found or already deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while soft deleting UserId {id}.", id);
                return StatusCode(500, ex.Message);
            }
        }

        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagenationResponseDTO<UserDTO>>> GetAllUsers(PagenationRequestDTO pagenation)
        {
            try
            {
                var users = await _userService.GetAllUsers(pagenation);
                return Ok(users);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No users found in the database.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting all users.");
                return StatusCode(500, ex.Message);
            }
        }

       
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User,Bus Operator")]
        public async Task<ActionResult<UserDTO>> GetUserById( int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "User with ID {Id} was not found.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting UserId {Id}.", id);
                return StatusCode(500, ex.Message);
            }
        }

        
        [HttpPost("register")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> RegisterUser( RegisterUserRequestDTO dto)
        {
            try
            {
                var result = await _userService.RegisterUser(dto);
                return CreatedAtAction(nameof(GetUserById), new { id = result.UserId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering user with email {Email}.", dto.Email);
                return BadRequest(ex.Message);
            }
        }

       
        [HttpPut("UpdateUser_profile")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateUserProfile(UpdateUserDTO dto)
        {
            try
            {
                var result = await _userService.UpdateUserProfile(dto);
                if (result)
                {
                    return NoContent();
                }
                return NotFound($"User with ID {dto.UserId} not found.");
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Update failed: UserId {UserId} not found.", dto.UserId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user profile for UserId: {UserId}.", dto.UserId);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
