using System.Security.Claims;
using FastxWebApi.Exceptions;
using FastxWebApi.Filter;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
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
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost("bookTicket")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> BookTicket(BookingRequestDTO bookingRequestDTO)
        {
            try
            {
                var result = await _bookingService.BookTicket(bookingRequestDTO);
                return Ok(result);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Booking failed: RouteId {RouteId} not found.", bookingRequestDTO.RouteId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while booking a ticket.");
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost("cancel/{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                var result = await _bookingService.CancelBooking(id);
                return Ok(result);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Cancellation failed for BookingId {id} because it was not found.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while canceling BookingId {id}.", id);
                return StatusCode(500, "An error occurred while canceling the booking.");
            }
        }

        [HttpPost("cancel-seats")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CancelSelectedSeats(CancelSeatsDTO dto)
        {
            try
            {
                var result = await _bookingService.CancelSelectedSeats(dto);
                return Ok(result);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Seat cancellation failed: BookingId {BookingId} not found.", dto.BookingId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while canceling seats for BookingId {BookingId}.", dto.BookingId);
                return StatusCode(500, "An error occurred while canceling the seats.");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,BusOperator")]
        public async Task<ActionResult<PagenationResponseDTO<BookingResponseDTO>>> GetAllBookings( PagenationRequestDTO pagenation)
        {
            try
            {
                var bookings = await _bookingService.GetAllBookings(pagenation);
                return Ok(bookings);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No bookings found in the collection.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching all bookings.");
                return StatusCode(500, "An error occurred while getting all bookings.");
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,BusOperator")]
        public async Task<ActionResult<BookingResponseDTO>> GetBookingById(int id)
        {
            try
            {
                var booking = await _bookingService.GetBookingById(id);
                return Ok(booking);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Booking with ID {id} not found.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting BookingId {id}.", id);
                return StatusCode(500, "An error occurred while getting the booking.");
            }
        }


        [HttpGet("getbookingsBybusId/{busId}")]
        [Authorize(Roles = "BusOperator,Admin")]
        public async Task<ActionResult<PagenationResponseDTO<BookingResponseDTO>>> GetBookingsByBusId(int busId, PagenationRequestDTO pagenation)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByBusId(busId, pagenation);
                return Ok(bookings);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No bookings found for BusId: {busId}.", busId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting bookings for BusId: {busId}.", busId);
                return StatusCode(500, "An error occurred while getting bookings by bus ID.");
            }
        }


        [HttpGet("GetBookingsbyuserId/{userId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<PagenationResponseDTO<BookingResponseDTO>>> GetBookingsByUser(int userId,  PagenationRequestDTO pagenation)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByUser(userId, pagenation);
                return Ok(bookings);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No bookings found for UserId: {userId}.", userId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting bookings for UserId: {userId}.", userId);
                return StatusCode(500, "An error occurred while getting bookings by user ID.");
            }
        }


        [HttpGet("{bookingId}/GetSeatsByBookingId")]
        [Authorize(Roles = "Admin,BusOperator")]
        public async Task<ActionResult<IEnumerable<SeatDTO>>> GetSeatsByBookingId(int bookingId, int userId)
        {
            try
            {
                var seats = await _bookingService.GetSeatsByBookingId(bookingId, userId);
                return Ok(seats);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No seats found for BookingId: {bookingId} and UserId: {userId}.", bookingId, userId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching seats for BookingId: {bookingId}.", bookingId);
                return StatusCode(500, "An error occurred while fetching seats.");
            }
        }

    }
}
