using System.Security.Claims;
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
    public class BusController : ControllerBase
    {
        private readonly IBusService _busService;
        private readonly ILogger<BusController> _logger;

        public BusController(IBusService busService, ILogger<BusController> logger)
        {
            _busService = busService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,BusOperator")]
        public async Task<IActionResult> AddBus(BusDTO busDTO)
        {
            var busOperatorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("Extracted busOperatorIdString from JWT: {busOperatorIdString}", busOperatorIdString);
            if (string.IsNullOrEmpty(busOperatorIdString) || !int.TryParse(busOperatorIdString, out int busOperatorId))
            {
                return Unauthorized("User ID claim not found or invalid.");
            }
            _logger.LogInformation("Parsed busOperatorId: {busOperatorId}", busOperatorId);
            try
            {
                var result = await _busService.AddBus(busDTO,busOperatorId);
                return CreatedAtAction(nameof(GetBusById), new { id = result.BusId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new bus.");
                return BadRequest(ex.Message);
            }
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,BusOperator")]
        public async Task<IActionResult> SoftDeleteBus(int id)
        {
            try
            {
                var result = await _busService.SoftDeleteBus(id);
                if (result)
                {
                    return NoContent(); 
                }
                return NotFound($"Bus with ID {id} not found or already deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting bus with ID {Id}.", id);
                return StatusCode(500, ex.Message);
            }
        }

        
        [HttpGet]
        [Authorize(Roles = "Admin,BusOperator")]
        public async Task<ActionResult<PagenationResponseDTO<BusDTO>>> GetAllBuses( PagenationRequestDTO pagenation)
        {
            try
            {
                var buses = await _busService.GetAllBuses(pagenation);
                return Ok(buses);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No buses found in the database.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting all buses.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,BusOperator")]
        public async Task<ActionResult<BusDTO>> GetBusById(int id)
        {
            try
            {
                var bus = await _busService.GetBusById(id);
                return Ok(bus);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Bus with ID {Id} was not found.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting bus with ID {Id}.", id);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,BusOperator")]
        public async Task<IActionResult> UpdateBus(int id, BusDTO busDTO)
        {
            if (id != busDTO.BusId)
            {
                return BadRequest("Bus ID in the URL does not match the ID in the request body.");
            }
            try
            {
                var result = await _busService.UpdateBus(id, busDTO);
                if (result)
                {
                    return NoContent(); 
                }
                return NotFound($"Bus with ID {id} not found.");
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Update failed: BusId {Id} not found.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating bus with ID {Id}.", id);
                return StatusCode(500, ex.Message);
            }
        }
    }




}
