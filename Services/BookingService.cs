using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastxWebApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRepository<int, BookingSeat> _bookingSeatRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IRefundRepository _refundRepository;
        private readonly ILogger<BookingService> _logger;
        private readonly IPaymentRepository _paymentRepository;


        public BookingService(
            IBookingRepository bookingRepository,
            IRepository<int, BookingSeat> bookingSeatRepository,
            IRefundRepository refundRepository,
            IRouteRepository routeRepository,
            ISeatRepository seatRepository,
            IMapper mapper,
            ApplicationDbContext context,
            ILogger<BookingService> logger,
            IPaymentRepository paymentRepository)
        {
            _bookingRepository = bookingRepository;
            _routeRepository = routeRepository;
            _seatRepository = seatRepository;
            _bookingSeatRepository = bookingSeatRepository;
            _mapper = mapper;
            _context = context;
            _logger = logger;
            _refundRepository = refundRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<string> BookTicket(BookingRequestDTO bookingRequestDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Attempting to book ticket for RouteId: {RouteId} by UserId: {UserId}", bookingRequestDTO.RouteId, bookingRequestDTO.UserId);

                var route = await _routeRepository.GetById(bookingRequestDTO.RouteId);
                if (route == null)
                {
                    _logger.LogWarning("Booking failed: RouteId {RouteId} not found.", bookingRequestDTO.RouteId);
                    throw new NoSuchEntityException("Route not found.");
                }

                var seatsToBook = await _context.Seats
                    .Where(s => s.RouteId == bookingRequestDTO.RouteId &&bookingRequestDTO.SeatNumbers.Contains(s.SeatNumber) &&!s.IsBooked)
                    .ToListAsync();

                if (seatsToBook.Count != bookingRequestDTO.SeatNumbers.Count)
                {
                    _logger.LogWarning("Booking failed: One or more requested seats are not available or do not exist.");
                    throw new InvalidActionException("One or more requested seats are not available or do not exist.");
                }

                var booking = _mapper.Map<Booking>(bookingRequestDTO);
                booking.NoOfSeats = seatsToBook.Count;
                booking.TotalFare = route.Fare * booking.NoOfSeats;
                booking.Status = "Booked";

                var addedBooking = await _bookingRepository.Add(booking);

                foreach (var seat in seatsToBook)
                {
                    var bookingSeat = new BookingSeat { BookingId = addedBooking.BookingId, SeatId = seat.SeatId };
                    await _bookingSeatRepository.Add(bookingSeat);
                    seat.IsBooked = true;
                    seat.Status = "Booked";
                    await _seatRepository.Update(seat);
                }

                var payment = new Payment
                {
                    BookingId = addedBooking.BookingId,
                    UserId = booking.UserId,
                    Amount = booking.TotalFare,
                    Status = "Success", // hardcoded for now
                    PaymentDate = DateTime.Now
                };
                await _paymentRepository.Add(payment);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Ticket booked successfully. Booking ID: {BookingId}", addedBooking.BookingId);
                return $"Ticket booked successfully. Booking ID: {addedBooking.BookingId}";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while booking ticket for RouteId: {RouteId} by UserId: {UserId}", bookingRequestDTO.RouteId, bookingRequestDTO.UserId);
                throw ;
            }
        }

        public async Task<string> CancelBooking(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Attempting to cancel booking with ID: {BookingId}", id);
                var booking = await _bookingRepository.GetById(id);

                if (booking == null)
                {
                    _logger.LogWarning("Cancellation failed: BookingId {BookingId} not found.", id);
                    throw new NoSuchEntityException("Booking not found.");
                }

                if (booking.Status == "Canceled" || booking.Status == "Refunded" || booking.IsDeleted)
                {
                    _logger.LogWarning("Cancellation failed: BookingId {BookingId} is already in status {Status} or deleted.", id, booking.Status);
                    return $"Booking with ID {id} has already been canceled, refunded, or deleted.";
                }
                var bookingSeats = await _context.BookingSeats
            .Include(bs => bs.Seat)
            .Where(bs => bs.BookingId == id)
            .ToListAsync();

                foreach (var bs in bookingSeats)
                {
                    bs.Seat.IsBooked = false;
                    bs.Seat.Status = "Available";
                    _context.BookingSeats.Remove(bs); // remove mapping
                }

                var softDeleteResult = await _bookingRepository.SoftDelete(id);
                if (!softDeleteResult)
                {
                    _logger.LogError("Failed to soft delete booking {BookingId}.", id);
                    throw new Exception("Cancellation failed.");
                }

                var newRefund = new Refund
                {
                    BookingId = booking.BookingId,
                    RefundAmount = booking.TotalFare,
                    Status = "Pending",
                    RefundDate = DateTime.Now,
                    UserId = booking.UserId
                };
                await _refundRepository.Add(newRefund);

                await transaction.CommitAsync();
                _logger.LogInformation("Booking {BookingId} successfully canceled. Refund request created.", id);
                return $"Booking with ID {id} successfully canceled. A refund request has been created.";
            }
            catch (NoSuchEntityException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while canceling booking with ID: {BookingId}", id);
                throw new Exception("An error occurred while canceling the booking.", ex);
            }
        }

        public async Task<string> CancelSelectedSeats(CancelSeatsDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Attempting to cancel seats for BookingId: {BookingId}", dto.BookingId);

                var booking = await _bookingRepository.GetById(dto.BookingId);
                if (booking == null)
                {
                    _logger.LogWarning("Seat cancellation failed: BookingId {BookingId} not found.", dto.BookingId);
                    throw new NoSuchEntityException("Booking not found.");
                }

                if (booking.Status == "Canceled" || booking.Status == "Refunded" || booking.IsDeleted)
                {
                    throw new InvalidActionException("Cannot cancel seats for a booking that is already canceled, refunded, or deleted.");
                }

                var bookingSeatsToCancel = await _context.BookingSeats.Include(bs => bs.Seat).Where(bs => bs.BookingId == dto.BookingId && dto.SeatIds.Contains(bs.SeatId)).ToListAsync();

                if (bookingSeatsToCancel.Count() != dto.SeatIds.Count)
                {
                    _logger.LogWarning("Seat cancellation failed: One or more requested seats not found or do not belong to BookingId {BookingId}.", dto.BookingId);
                    throw new NoSuchEntityException("One or more requested seats not found or do not belong to the booking.");
                }

                var seatsToUpdate = new List<Seat>();
                foreach (var bookingSeat in bookingSeatsToCancel)
                {
                    _context.BookingSeats.Remove(bookingSeat);
                    bookingSeat.Seat.IsBooked = false;
                    bookingSeat.Seat.Status = "Available";
                    seatsToUpdate.Add(bookingSeat.Seat);
                }

                booking.NoOfSeats -= seatsToUpdate.Count;
                var route = await _routeRepository.GetById(booking.RouteId);
                booking.TotalFare = route.Fare * booking.NoOfSeats;

                await _bookingRepository.Update(booking);
                await _seatRepository.UpdateAll(seatsToUpdate);
                await transaction.CommitAsync();

                _logger.LogInformation("Seats for BookingId {BookingId} cancelled successfully.", dto.BookingId);
                return $"Seats for BookingId {dto.BookingId} cancelled successfully.";
            }
            catch (NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while cancelling seats for BookingId: {BookingId}", dto.BookingId);
                throw new Exception("Error cancelling seats.", ex);
            }
        }

        public async Task<PagenationResponseDTO<BookingResponseDTO>> GetAllBookings(PagenationRequestDTO pagination)
        {
            _logger.LogInformation("Fetching all bookings with pagination.");

            var pagedResult = await _bookingRepository.GetAll(pagination);

            if (pagedResult == null || !pagedResult.Items.Any())
            {
                _logger.LogWarning("No bookings found in the collection.");
                throw new NoEntriessInCollectionException("No bookings found.");
            }

            var bookingDtos = _mapper.Map<IEnumerable<BookingResponseDTO>>(pagedResult.Items);

            return new PagenationResponseDTO<BookingResponseDTO>
            {
                Items = bookingDtos,
                TotalCount = pagedResult.TotalCount,
                PageSize = pagedResult.PageSize,
                CurrentPage = pagedResult.CurrentPage
            };
        }

        public async Task<BookingResponseDTO> GetBookingById(int id)
        {
            _logger.LogInformation("Fetching booking by ID: {id}", id);
            var booking = await _bookingRepository.GetById(id);

            if (booking == null)
            {
                _logger.LogWarning("Booking with ID {id} not found.", id);
                throw new NoSuchEntityException($"Booking with ID {id} not found.");
            }

            return _mapper.Map<BookingResponseDTO>(booking);
        }

        public async Task<PagenationResponseDTO<BookingResponseDTO>> GetBookingsByBusId(int busId, PagenationRequestDTO pagination)
        {
            _logger.LogInformation("Fetching bookings for BusId: {BusId}", busId);
            var pagedResult = await _bookingRepository.GetBookingsByBusId(busId, pagination);

            if (pagedResult == null || !pagedResult.Items.Any())
            {
                _logger.LogWarning("No bookings found for BusId: {BusId}.", busId);
                throw new NoEntriessInCollectionException("No bookings found for this bus.");
            }

            var bookingDtos = _mapper.Map<IEnumerable<BookingResponseDTO>>(pagedResult.Items);

            return new PagenationResponseDTO<BookingResponseDTO>
            {
                Items = bookingDtos,
                TotalCount = pagedResult.TotalCount,
                PageSize = pagedResult.PageSize,
                CurrentPage = pagedResult.CurrentPage
            };
        }

        public async Task<PagenationResponseDTO<BookingResponseDTO>> GetBookingsByUser(int userId, PagenationRequestDTO pagination)
        {
            _logger.LogInformation("Fetching bookings for UserId: {UserId}", userId);
            var pagedResult = await _bookingRepository.GetBookingsByUserId(userId, pagination);

            if (pagedResult == null || !pagedResult.Items.Any())
            {
                _logger.LogWarning("No bookings found for UserId: {UserId}.", userId);
                throw new NoEntriessInCollectionException("No bookings found for this user.");
            }

            var bookingDtos = _mapper.Map<IEnumerable<BookingResponseDTO>>(pagedResult.Items);

            return new PagenationResponseDTO<BookingResponseDTO>
            {
                Items = bookingDtos,
                TotalCount = pagedResult.TotalCount,
                PageSize = pagedResult.PageSize,
                CurrentPage = pagedResult.CurrentPage
            };
        }

        public async Task<IEnumerable<SeatDTO>> GetSeatsByBookingId(int bookingId, int userId)
        {
            _logger.LogInformation("Fetching seats for BookingId: {BookingId} by UserId: {UserId}", bookingId, userId);
            var seats = await _bookingRepository.GetSeatsByBookingId(bookingId, userId);

            if (seats == null || !seats.Any())
            {
                _logger.LogWarning("No seats found for BookingId: {BookingId} and UserId: {UserId}.", bookingId, userId);
                throw new NoEntriessInCollectionException("No seats found for this booking.");
            }

            return _mapper.Map<IEnumerable<SeatDTO>>(seats);
        }

        public async Task<string> SoftDeleteBooking(int id)
        {
            return await CancelBooking(id);
        }

    }
}