<<<<<<< HEAD:FastxWebApi-backend/PaymentService.cs
﻿using AutoMapper;
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

        public async Task<PagenationResponseDTO<PaymentDisplayDTO>> GetAllPayments(PagenationRequestDTO pagenation)
        {
            try
            {
                _logger.LogInformation("Fetching paginated payments.");

                var pagedPayments = await _paymentRepository.GetAll(pagenation);

                if (pagedPayments == null || !pagedPayments.Items.Any())
                {
                    _logger.LogWarning("No payments found.");
                    throw new NoEntriessInCollectionException("No payments found.");
                }

                var mappedItems = _mapper.Map<IEnumerable<PaymentDisplayDTO>>(pagedPayments.Items);

                var response = new PagenationResponseDTO<PaymentDisplayDTO>
                {
                    Items = mappedItems,
                    TotalCount = pagedPayments.TotalCount,
                    PageSize = pagedPayments.PageSize,
                    CurrentPage = pagedPayments.CurrentPage
                };

                _logger.LogInformation("Successfully fetched paginated payments.");
                return response;
            }
            catch (NoEntriessInCollectionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting paginated payments.");
                throw new Exception("Error getting paginated payments.", ex);
            }
        }

        public async Task<PaymentDisplayDTO> GetPaymentByBookingId(int bookingId)
        {
            try
            {
                _logger.LogInformation("Fetching payment for BookingId: {BookingId}", bookingId);
                var payment = await _paymentRepository.GetPaymentByBookingId(bookingId);

                if (payment == null)
                {
                    _logger.LogWarning("Payment for BookingId {BookingId} not found.", bookingId);
                    throw new NoSuchEntityException($"Payment for BookingId {bookingId} not found.");
                }

                _logger.LogInformation("Successfully retrieved payment for BookingId: {BookingId}", bookingId);
                return _mapper.Map<PaymentDisplayDTO>(payment);
            }
            catch (NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting payment for BookingId: {BookingId}", bookingId);
                throw new Exception("Error getting Payment by booking id", ex);
            }
        }

        public async Task<PagenationResponseDTO<PaymentDisplayDTO>> GetPaymentsByUserId(int userId, PagenationRequestDTO pagenation)
        {
            try
            {
                _logger.LogInformation("Fetching paginated payments for user with ID {userId}.");
                var pagedPayments = await _paymentRepository.GetPaymentsByUserId(userId, pagenation);

                if (pagedPayments == null || !pagedPayments.Items.Any())
                {
                    _logger.LogWarning("No payments found for user with ID {userId}.");
                    throw new NoEntriessInCollectionException("No payments found for this user.");
                }

                var mappedItems = _mapper.Map<IEnumerable<PaymentDisplayDTO>>(pagedPayments.Items);

                var response = new PagenationResponseDTO<PaymentDisplayDTO>
                {
                    Items = mappedItems,
                    TotalCount = pagedPayments.TotalCount,
                    PageSize = pagedPayments.PageSize,
                    CurrentPage = pagedPayments.CurrentPage
                };

                _logger.LogInformation("Successfully fetched paginated payments for user.");
                return response;
            }
            catch (NoEntriessInCollectionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting paginated payments for user {userId}.");
                throw new Exception("Error getting paginated payments for user.", ex);
            }
        }

        public async Task<string> ProcessPayment(PaymentDTO paymentDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Attempting to process payment for BookingId: {BookingId}", paymentDTO.BookingId);
                var booking = await _bookingRepository.GetById(paymentDTO.BookingId);

                if (booking == null)
                {
                    _logger.LogWarning("Payment failed: BookingId {BookingId} not found.", paymentDTO.BookingId);
                    throw new NoSuchEntityException("Booking not found.");
                }

                if (booking.Status == "Completed")
                {
                    _logger.LogWarning("Payment already processed for BookingId: {BookingId}.", paymentDTO.BookingId);
                    return "Payment already processed for this booking.";
                }

                var payment = _mapper.Map<Payment>(paymentDTO);

                booking.Status = "Completed";
                await _bookingRepository.Update(booking); 

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
            catch (NoSuchEntityException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while processing payment for BookingId: {BookingId}", paymentDTO.BookingId);
                throw new Exception("Error processing payments.", ex);
            }
        }
    }
}
=======
﻿using AutoMapper;
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

        public async Task<PagenationResponseDTO<PaymentDisplayDTO>> GetAllPayments(PagenationRequestDTO pagenation)
        {
            try
            {
                _logger.LogInformation("Fetching paginated payments.");

                var pagedPayments = await _paymentRepository.GetAll(pagenation);

                if (pagedPayments == null || !pagedPayments.Items.Any())
                {
                    _logger.LogWarning("No payments found.");
                    throw new NoEntriessInCollectionException("No payments found.");
                }

                var mappedItems = _mapper.Map<IEnumerable<PaymentDisplayDTO>>(pagedPayments.Items);

                var response = new PagenationResponseDTO<PaymentDisplayDTO>
                {
                    Items = mappedItems,
                    TotalCount = pagedPayments.TotalCount,
                    PageSize = pagedPayments.PageSize,
                    CurrentPage = pagedPayments.CurrentPage
                };

                _logger.LogInformation("Successfully fetched paginated payments.");
                return response;
            }
            catch (NoEntriessInCollectionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting paginated payments.");
                throw new Exception("Error getting paginated payments.", ex);
            }
        }

        public async Task<PaymentDisplayDTO> GetPaymentByBookingId(int bookingId)
        {
            try
            {
                _logger.LogInformation("Fetching payment for BookingId: {BookingId}", bookingId);
                var payment = await _paymentRepository.GetPaymentByBookingId(bookingId);

                if (payment == null)
                {
                    _logger.LogWarning("Payment for BookingId {BookingId} not found.", bookingId);
                    throw new NoSuchEntityException($"Payment for BookingId {bookingId} not found.");
                }

                _logger.LogInformation("Successfully retrieved payment for BookingId: {BookingId}", bookingId);
                return _mapper.Map<PaymentDisplayDTO>(payment);
            }
            catch (NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting payment for BookingId: {BookingId}", bookingId);
                throw new Exception("Error getting Payment by booking id", ex);
            }
        }

        public async Task<PagenationResponseDTO<PaymentDisplayDTO>> GetPaymentsByUserId(int userId, PagenationRequestDTO pagenation)
        {
            try
            {
                _logger.LogInformation("Fetching paginated payments for user with ID {userId}.");
                var pagedPayments = await _paymentRepository.GetPaymentsByUserId(userId, pagenation);

                if (pagedPayments == null || !pagedPayments.Items.Any())
                {
                    _logger.LogWarning("No payments found for user with ID {userId}.");
                    throw new NoEntriessInCollectionException("No payments found for this user.");
                }

                var mappedItems = _mapper.Map<IEnumerable<PaymentDisplayDTO>>(pagedPayments.Items);

                var response = new PagenationResponseDTO<PaymentDisplayDTO>
                {
                    Items = mappedItems,
                    TotalCount = pagedPayments.TotalCount,
                    PageSize = pagedPayments.PageSize,
                    CurrentPage = pagedPayments.CurrentPage
                };

                _logger.LogInformation("Successfully fetched paginated payments for user.");
                return response;
            }
            catch (NoEntriessInCollectionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting paginated payments for user {userId}.");
                throw new Exception("Error getting paginated payments for user.", ex);
            }
        }

        public async Task<string> ProcessPayment(PaymentDTO paymentDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Attempting to process payment for BookingId: {BookingId}", paymentDTO.BookingId);
                var booking = await _bookingRepository.GetById(paymentDTO.BookingId);

                if (booking == null)
                {
                    _logger.LogWarning("Payment failed: BookingId {BookingId} not found.", paymentDTO.BookingId);
                    throw new NoSuchEntityException("Booking not found.");
                }

                if (booking.Status == "Completed")
                {
                    _logger.LogWarning("Payment already processed for BookingId: {BookingId}.", paymentDTO.BookingId);
                    return "Payment already processed for this booking.";
                }

                var payment = _mapper.Map<Payment>(paymentDTO);

                booking.Status = "Completed";
                await _bookingRepository.Update(booking); 

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
            catch (NoSuchEntityException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while processing payment for BookingId: {BookingId}", paymentDTO.BookingId);
                throw new Exception("Error processing payments.", ex);
            }
        }
    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Services/PaymentService.cs
