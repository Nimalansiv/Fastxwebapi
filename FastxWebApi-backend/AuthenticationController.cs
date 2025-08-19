using FastxWebApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Exceptions;
using Microsoft.AspNetCore.Cors;


namespace FastxWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("DefaultCORS")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticate _authenticateService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticate authenticateService, ILogger<AuthenticationController> logger)
        {
            _authenticateService = authenticateService;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterUserResponseDTO>> Register(RegisterUserRequestDTO requestDTO)
        {
            try
            {
                var result = await _authenticateService.RegisterUser(requestDTO);

                return CreatedAtAction(nameof(Login), new { id = result.UserId }, result);
            }
            catch (UserAlreadyExistsException ex)
            {
                _logger.LogWarning(ex, "Registration failed: user with email {Email} already present.", requestDTO.Email);

                return BadRequest(new ErrorObjectDTOcs { ErrorNumber = 400, ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during user registration.");
                return BadRequest(new ErrorObjectDTOcs { ErrorNumber = 400, ErrorMessage = "Registration failed." });
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO requestDTO)
        {
            try
            {
                var result = await _authenticateService.Login(requestDTO);
                return Ok(result);
            }
            catch (InvalidCredentialsException ex)
            {
                _logger.LogWarning(ex, "Login failed for user {Email}.", requestDTO.Email);
                return Unauthorized(new ErrorObjectDTOcs { ErrorNumber = 401, ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during login for user {Email}.", requestDTO.Email);
                return StatusCode(500, new ErrorObjectDTOcs { ErrorNumber = 500, ErrorMessage = "An error occurred during login." });
            }
        }
    }
}
