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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("process_Payment")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ProcessPayment(PaymentDTO paymentDTO)
        {
            try
            {
                var result = await _paymentService.ProcessPayment(paymentDTO);
                return Ok(result);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Payment process failed: BookingId {BookingId} not found.", paymentDTO.BookingId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while processing payment for BookingId: {BookingId}.", paymentDTO.BookingId);
                return StatusCode(500, ex.Message);
            }
        }

        
        [HttpGet("Get_allpayments")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagenationResponseDTO<PaymentDisplayDTO>>> GetAllPayments([FromQuery] PagenationRequestDTO pagenation)
        {
            try
            {
                var payments = await _paymentService.GetAllPayments(pagenation);
                return Ok(payments);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No payments found in the database.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting all payments.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetPaymentByBookingId/{bookingId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<PaymentDisplayDTO>> GetPaymentByBookingId([FromRoute] int bookingId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByBookingId(bookingId);
                return Ok(payment);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Payment for BookingId {BookingId} not found.", bookingId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting payment for BookingId {BookingId}.", bookingId);
                return StatusCode(500, ex.Message);
            }
        }

        
        [HttpGet("GetPaymentsby_userId/{userId}")]
        
        public async Task<ActionResult<PagenationResponseDTO<PaymentDisplayDTO>>> GetPaymentsByUserId([FromRoute]int userId, [FromQuery] PagenationRequestDTO pagenation)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByUserId(userId, pagenation);
                return Ok(payments);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No payments found for UserId: {UserId}.", userId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting payments for UserId: {UserId}.", userId);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
