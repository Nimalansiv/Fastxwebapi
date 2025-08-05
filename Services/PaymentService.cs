using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace FastxWebApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;


        public PaymentService(IPaymentRepository paymentRepository, IBookingRepository bookingRepository, IMapper mapper, ApplicationDbContext context, ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        async Task<IEnumerable<PaymentDisplayDTO>> IPaymentService.GetAllPayments()
        {
            try
            {
                _logger.LogInformation("Fetching all payments.");

                var payments = await _paymentRepository.GetAll();
                if (payments == null || !payments.Any())
                {
                    _logger.LogWarning("No payments found in the collection.");

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved all payments.");

                return _mapper.Map<IEnumerable<PaymentDisplayDTO>>(payments);
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all payments.");

                throw new Exception("Error getting all payments");
            }
        }

        async Task<PaymentDisplayDTO> IPaymentService.GetPaymentByBookingId(int bookingId)
        {
            try
            {
                _logger.LogInformation("Fetching payment for BookingId: {BookingId}", bookingId);

                var payment = await _paymentRepository.GetPaymentByBookingId(bookingId);
                if (payment == null)
                {
                    _logger.LogWarning("Payment for BookingId {BookingId} not found.", bookingId);
                    throw new NoSuchEntityException();
                }
                _logger.LogInformation("Successfully retrieved payment for BookingId: {BookingId}", bookingId);

                return _mapper.Map<PaymentDisplayDTO>(payment);
            }
            catch(NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting payment for BookingId: {BookingId}", bookingId);

                throw new Exception("Error  getting Payment by booking id");
            }
        }

        async Task<IEnumerable<PaymentDisplayDTO>> IPaymentService.GetPaymentsByUserId(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching payments for UserId: {UserId}", userId);

                var payments = await _paymentRepository.GetPaymentsByUserId(userId);
                if (payments == null || !payments.Any())
                {
                    _logger.LogWarning("No payments found for UserId: {UserId}.", userId);

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved payments for UserId: {UserId}", userId);

                return _mapper.Map<IEnumerable<PaymentDisplayDTO>>(payments);
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred while getting payments for UserId: {UserId}", userId);

                throw new Exception("Error getting Payments by user");
            }
        }

       async Task<string> IPaymentService.ProcessPayment(PaymentDTO paymentDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Attempting to process payment for BookingId: {BookingId}", paymentDTO.BookingId);

                var booking = await _bookingRepository.GetById(paymentDTO.BookingId);
                if (booking == null)
                {
                    _logger.LogWarning("Payment failed: BookingId {BookingId} not found.", paymentDTO.BookingId);

                    throw new NoSuchEntityException();
                }


                if (booking.Status == "Completed")
                {
                    _logger.LogWarning("Payment already processed for BookingId: {BookingId}.", paymentDTO.BookingId);

                    return "Payment already processed for this booking.";
                }

                var payment = _mapper.Map<Payment>(paymentDTO);

                booking.Status = "Completed";
                await _bookingRepository.Update(booking.BookingId, booking);

                var addedPayment = await _paymentRepository.Add(payment);

                if (addedPayment == null)
                {
                    _logger.LogError("Payment failed: Database operation returned null.");

                    throw new Exception("Failed to process payment.");
                }
                await transaction.CommitAsync();
                _logger.LogInformation("Payment for BookingId {BookingId} processed successfully. PaymentId: {PaymentId}", paymentDTO.BookingId, addedPayment.PaymentId);

                return $"Payment for BookingId {paymentDTO.BookingId} processed successfully.";
            }
            catch(NoSuchEntityException)
            {
                await transaction.RollbackAsync();

                throw;
            }
            catch(Exception e)
            {
                await transaction.RollbackAsync();

                _logger.LogError(e, "An error occurred while processing payment for BookingId: {BookingId}", paymentDTO.BookingId);
                throw new Exception("error processing payments");
            }




        }
    }
}
