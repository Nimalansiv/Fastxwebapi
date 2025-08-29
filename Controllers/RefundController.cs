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
    public class RefundController : ControllerBase
    {
        private readonly IRefundService _refundService;
        private readonly ILogger<RefundController> _logger;

        public RefundController(IRefundService refundService, ILogger<RefundController> logger)
        {
            _refundService = refundService;
            _logger = logger;
        }

        [HttpPost("approve/{refundId}")]
        [Authorize(Roles = "Admin,Bus Operator")]
        public async Task<IActionResult> ApproveRefund(int refundId)
        {
            var processedByUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _logger.LogInformation("BusOperatorId received: {busOperatorId}", processedByUserId);
            try
            {
              
                var result = await _refundService.ApproveRefund(refundId, processedByUserId);
                return Ok(result);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Refund approval failed: RefundId {RefundId} not found.", refundId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while approving refund with ID: {RefundId}.", refundId);
                return StatusCode(500, ex.Message);
            }
        }

       
        [HttpGet]
        [Authorize(Roles = "Admin,Bus Operator")]
        public async Task<ActionResult<PagenationResponseDTO<RefundDTO>>> GetAllRefunds([FromQuery] PagenationRequestDTO pagenation)
        {
            try
            {
                var refunds = await _refundService.GetAllRefunds(pagenation);
                return Ok(refunds);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No refunds found in the database.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting all refunds.");
                return StatusCode(500, ex.Message);
            }
        }

        
        [HttpGet("GetpendingRefunds")]
        [Authorize(Roles = "Admin,Bus Operator")]
        public async Task<ActionResult<PagenationResponseDTO<RefundDTO>>> GetPendingRefunds([FromQuery]PagenationRequestDTO pagenation)
        {
            try
            {
                var pendingRefunds = await _refundService.GetPendingRefunds(pagenation);
                return Ok(pendingRefunds);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No pending refunds found in the database.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting pending refunds.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetRefundBybookingId/{bookingId}")]
        [Authorize(Roles = "Admin,User,Bus Operator")]
        public async Task<ActionResult<RefundDTO>> GetRefundByBookingId(int bookingId)
        {
            try
            {
                var refund = await _refundService.GetRefundByBookingId(bookingId);
                return Ok(refund);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Refund for BookingId {BookingId} not found.", bookingId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting refund for BookingId: {BookingId}.", bookingId);
                return StatusCode(500, ex.Message);
            }
        }

       
        [HttpGet("GetRefundsByUserId/{userId}")]
        [Authorize(Roles = "Admin,User,Bus Operator")]
        public async Task<ActionResult<PagenationResponseDTO<RefundDTO>>> GetRefundsByUserId(int userId, [FromQuery]PagenationRequestDTO pagenation)
        {
            try
            {
                var refunds = await _refundService.GetRefundsByUserId(userId, pagenation);
                return Ok(refunds);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No refunds found for UserId: {UserId}.", userId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting refunds for UserId: {UserId}.", userId);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
