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
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;
        private readonly ILogger<SeatController> _logger;

        public SeatController(ISeatService seatService, ILogger<SeatController> logger)
        {
            _seatService = seatService;
            _logger = logger;
        }

       
        [HttpGet("by-route/{routeId}/available")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<PagenationResponseDTO<SeatDTO>>> GetAvailableSeatsByRoute(
             int routeId,
             [FromQuery] PagenationRequestDTO pagination)
        {
            try
            {
                var seats = await _seatService.GetSeatsByRoute(routeId, pagination);
                return Ok(seats);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No available seats found for RouteId: {RouteId}.", routeId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting available seats for RouteId: {RouteId}", routeId);
                return StatusCode(500, ex.Message);
            }
        }

       
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDeleteSeat(int id)
        {
            try
            {
                var result = await _seatService.SoftDeleteSeat(id);
                if (result)
                {
                    return NoContent(); 
                }
                return NotFound($"Seat with ID {id} not found or already deleted.");
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Soft delete failed: Seat with ID {Id} not found.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting seat with ID {Id}.", id);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("BookSeats/{routeId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> BookSeats(int routeId, [FromBody] List<string> seatNumbers)
        {
            try
            {
                await _seatService.BookSeats(routeId, seatNumbers);
                return Ok("Seats booked successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error booking seats for route {RouteId}", routeId);
                return BadRequest(ex.Message);
            }
        }


    }
}
