using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastxWebApi.Services
{
    public class RefundService : IRefundService
    {

        private readonly IRefundRepository _refundRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RefundService> _logger;


        public RefundService(IRefundRepository refundRepository, IBookingRepository bookingRepository, IUserRepository userRepository, IMapper mapper, ApplicationDbContext context, ILogger<RefundService> logger)
        {
            _refundRepository = refundRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<string> ApproveRefund(int refundId, int ProcessedByUserId)
        {
            if (ProcessedByUserId <= 0)
            {
                _logger.LogWarning("Invalid ProcessedByUserId provided: {ProcessedByUserId}", ProcessedByUserId);
                throw new InvalidActionException("Invalid ProcessedByUserId provided.");
            }
            var refund = await _refundRepository.GetById(refundId);
            if (refund == null)
            {
                _logger.LogWarning("Refund approval failed: RefundId {RefundId} not found.", refundId);
                throw new NoSuchEntityException($"Refund with ID {refundId} not found.");
            }

            if (refund.Status != "Pending")
            {
                _logger.LogWarning("Refund approval failed: RefundId {RefundId} status is not Pending.", refundId);
                throw new InvalidActionException($"Refund with ID {refundId} cannot be approved. Current status is {refund.Status}.");
            }

            var processedByUser = await _userRepository.GetById(ProcessedByUserId);
            if (processedByUser == null || processedByUser.RoleId != 2)
            {
                _logger.LogWarning("Refund approval failed: Invalid ProcessedByUserId {ProcessedByUserId}.", ProcessedByUserId);
                throw new InvalidActionException("Invalid ProcessedByUserId provided.");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Attempting to approve refund with ID: {RefundId} by user: {ProcessedByUserId}", refundId, ProcessedByUserId);

                refund.Status = "Approved";
                refund.ProcessedBy = ProcessedByUserId;
                refund.RefundDate = DateTime.Now;
                await _refundRepository.Update(refund);

                var booking = await _bookingRepository.GetById(refund.BookingId);
                if (booking != null)
                {
                    booking.Status = "Refunded";
                    await _bookingRepository.Update(booking);
                }

                await transaction.CommitAsync();
                _logger.LogInformation("Refund with ID {RefundId} approved successfully.", refundId);
                return "Refund approved successfully.";
            }
            catch (NoSuchEntityException)
            {
                
                throw;
            }
            catch (InvalidActionException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
               
                await transaction.RollbackAsync();
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }

                _logger.LogError(ex, "Error approving refund with ID {RefundId}: {ErrorMessage}", refundId, errorMessage);

                throw new Exception(errorMessage, ex);
            }
        }

        public async Task<PagenationResponseDTO<RefundDTO>> GetAllRefunds(PagenationRequestDTO pagenation)
        {
            _logger.LogInformation("Fetching all refunds with pagination.");
            var refundsPaged = await _refundRepository.GetAll(pagenation);

            if (refundsPaged == null || !refundsPaged.Items.Any())
            {
                _logger.LogWarning("No refunds found in the collection.");
                throw new NoEntriessInCollectionException();
            }

            var mapped = _mapper.Map<IEnumerable<RefundDTO>>(refundsPaged.Items);

            return new PagenationResponseDTO<RefundDTO>
            {
                Items = mapped,
                TotalCount = refundsPaged.TotalCount,
                CurrentPage = pagenation.PageNumber,
                PageSize = pagenation.PageSize
            };
        }

        public async Task<PagenationResponseDTO<RefundDTO>> GetPendingRefunds(PagenationRequestDTO pagenation)
        {
            _logger.LogInformation("Fetching pending refunds.");
            var refundsPaged = await _refundRepository.GetPendingRefunds(pagenation);

            if (refundsPaged == null || !refundsPaged.Items.Any())
            {
                _logger.LogWarning("No pending refunds found.");
                throw new NoEntriessInCollectionException();
            }

            var mapped = _mapper.Map<IEnumerable<RefundDTO>>(refundsPaged.Items);

            return new PagenationResponseDTO<RefundDTO>
            {
                Items = mapped,
                TotalCount = refundsPaged.TotalCount,
                CurrentPage = pagenation.PageNumber,
                PageSize = pagenation.PageSize
            };
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
            catch (NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting refund for BookingId: {BookingId}", bookingId);
                throw new Exception("Error getting refunds by booking Id", ex);
            }
        }

        public async Task<PagenationResponseDTO<RefundDTO>> GetRefundsByUserId(int userId, PagenationRequestDTO pagenation)
        {
            _logger.LogInformation("Fetching refunds by user ID.");
            var refundsPaged = await _refundRepository.GetRefundsByUserId(userId, pagenation);

            if (refundsPaged == null || !refundsPaged.Items.Any())
            {
                _logger.LogWarning("No refunds found for user ID: {UserId}.", userId);
                throw new NoEntriessInCollectionException();
            }

            var mapped = _mapper.Map<IEnumerable<RefundDTO>>(refundsPaged.Items);

            return new PagenationResponseDTO<RefundDTO>
            {
                Items = mapped,
                TotalCount = refundsPaged.TotalCount,
                CurrentPage = pagenation.PageNumber,
                PageSize = pagenation.PageSize
            };
        }
    }
}
