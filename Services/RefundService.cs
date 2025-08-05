using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastxWebApi.Services
{
    public class RefundService:IRefundService
    {

        private readonly IRefundRepository _refundRepository;
        private readonly IBookingRepository _bookingRepository; 
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RefundService> _logger;
        public RefundService(IRefundRepository refundRepository, IBookingRepository bookingRepository, IMapper mapper, ApplicationDbContext context, ILogger<RefundService> logger)
        {
            _refundRepository = refundRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<string> ApproveRefund(int refundId, int ProcessedByUserId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Attempting to approve refund with ID: {RefundId} by user: {ProcessedByUserId}", refundId, ProcessedByUserId);

                var refund = await _refundRepository.GetById(refundId);
                if (refund == null)
                {
                    _logger.LogWarning("Refund approval failed: RefundId {RefundId} not found.", refundId);

                    throw new NoSuchEntityException();
                }

                // Add a check to ensure only pending refunds can be approved
                if (refund.Status != "Pending")
                {
                    _logger.LogWarning("Refund approval failed: RefundId {RefundId} status is not Pending.", refundId);

                    return $"Refund with ID {refundId} cannot be approved. Current status is {refund.Status}.";
                }

                // Step 1: Update the refund record
                refund.Status = "Approved";
                refund.ProcessedBy = ProcessedByUserId;
                refund.RefundDate = DateTime.Now;
                await _refundRepository.Update(refundId, refund);

                // Step 2: Update the corresponding booking status
                var booking = await _bookingRepository.GetById(refund.BookingId);
                if (booking != null)
                {
                    booking.Status = "Refunded";
                    await _bookingRepository.Update(booking.BookingId, booking);
                }

                // Commit the transaction
                await transaction.CommitAsync();
                _logger.LogInformation("Refund with ID {RefundId} approved successfully.", refundId);


                return "Refund approved successfully.";
            }
            catch (NoSuchEntityException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while approving refund with ID: {RefundId}", refundId);

                throw new Exception("Error approving refund.", ex);
            }
        }


        public async Task<IEnumerable<RefundDTO>> GetAllRefunds()
        {
            try
            {
                _logger.LogInformation("Fetching all refunds.");

                var refunds = await _refundRepository.GetAll();
                if (refunds == null || !refunds.Any())
                {
                    _logger.LogWarning("No refunds found in the collection.");

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved all refunds.");

                return _mapper.Map<IEnumerable<RefundDTO>>(refunds);
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all refunds.");

                throw new Exception("error getting all refunds");
            }
        }

        public async Task<IEnumerable<RefundDTO>> GetPendingRefunds()
        {
            try
            {
                _logger.LogInformation("Fetching pending refunds.");

                var pendingRefunds = await _refundRepository.GetPendingRefunds();
                if (pendingRefunds == null || !pendingRefunds.Any())
                {
                    _logger.LogWarning("No pending refunds found in the collection.");

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved pending refunds.");

                return _mapper.Map<IEnumerable<RefundDTO>>(pendingRefunds);
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch( Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting pending refunds.");

                throw new Exception("error getting pending refunds");
            }
        }

        public async Task<RefundDTO> GetRefundByBookingId(int bookingId)
        {
            try
            {
                _logger.LogInformation("Fetching refund for BookingId: {BookingId}", bookingId);

                var refund = await _refundRepository.GetRefundByBookingId(bookingId);
                if (refund == null)
                {
                    _logger.LogWarning("Refund for BookingId {BookingId} not found.", bookingId);

                    throw new NoSuchEntityException();
                }
                _logger.LogInformation("Successfully retrieved refund for BookingId: {BookingId}", bookingId);

                return _mapper.Map<RefundDTO>(refund);
            }
            catch(NoSuchEntityException)
            {
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting refund for BookingId: {BookingId}", bookingId);

                throw new Exception("Error getting refunds by booking Id");
            }
        }

        public async Task<IEnumerable<RefundDTO>> GetRefundsByUserId(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching refunds for UserId: {UserId}", userId);

                var refunds = await _refundRepository.GetRefundsByUserId(userId);
                if (refunds == null || !refunds.Any())
                {
                    _logger.LogWarning("No refunds found for UserId: {UserId}.", userId);

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved refunds for UserId: {UserId}", userId);

                return _mapper.Map<IEnumerable<RefundDTO>>(refunds);
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting refunds for UserId: {UserId}", userId);

                throw new Exception("Error getting Refunds by user ID");
            }
        }
    }
}
