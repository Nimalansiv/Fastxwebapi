using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Repository;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Services
{
    public class BookingService:IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRepository<int, BookingSeat> _bookingSeatRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IRefundRepository _refundRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IBookingRepository bookingRepository, IRepository<int, BookingSeat> bookingSeatRepository, IRefundRepository refundRepository,IRouteRepository routeRepository,ISeatRepository seatRepository,IMapper mapper, ApplicationDbContext context, ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _routeRepository = routeRepository;
            _seatRepository = seatRepository;
            _bookingSeatRepository = bookingSeatRepository;
            _mapper = mapper;
            _context = context;
            _logger = logger;
            _refundRepository = refundRepository;

        }

        async Task<string> IBookingService.BookTicket(BookingRequestDTO bookingRequestDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Attempting to book ticket for RouteId: {RouteId} by UserId: {UserId}", bookingRequestDTO.RouteId, bookingRequestDTO.UserId);
                var route = await _routeRepository.GetById(bookingRequestDTO.RouteId);
                if (route == null)
                {
                    _logger.LogWarning("Booking failed: RouteId {RouteId} not found.", bookingRequestDTO.RouteId);

                    throw new NoSuchEntityException();
                }
                var allSeatsOnRoute = await _seatRepository.GetSeatsByRoute(bookingRequestDTO.RouteId);

                
                var seatsToBook = allSeatsOnRoute.Where(s => bookingRequestDTO.SeatNumbers.Contains(s.SeatNumber)).ToList();

                if (seatsToBook.Count != bookingRequestDTO.SeatNumbers.Count)
                {
                    _logger.LogWarning("Booking failed: One or more requested seats are not available or do not exist for RouteId: {RouteId}", bookingRequestDTO.RouteId);

                    throw new Exception("one or more requested seats are not available or do not exist.");
                }

                var booking = _mapper.Map<Booking>(bookingRequestDTO);
                booking.NoOfSeats = seatsToBook.Count;
                booking.TotalFare = route.Fare * booking.NoOfSeats;

                var addedBooking = await _bookingRepository.Add(booking);
                if (addedBooking == null)
                {
                    _logger.LogError("Failed to book ticket. Database operation failed.");

                    throw new Exception("Failed to book ticket.");
                }

                foreach (var seat in seatsToBook)
                {
                    var bookingSeat = new BookingSeat
                    {
                        BookingId = addedBooking.BookingId,
                        SeatId = seat.SeatId
                    };
                    await _bookingSeatRepository.Add(bookingSeat);
                }
                await transaction.CommitAsync();
                _logger.LogInformation("Ticket booked successfully. Booking ID: {BookingId}", addedBooking.BookingId);
                return $"Ticket booked successfully. Booking ID: {addedBooking.BookingId}";

                return $"Ticket booked successfully. Booking ID: {addedBooking.BookingId}";
            }
            catch(NoSuchEntityException)
            {
                await transaction.RollbackAsync();

                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                _logger.LogError(ex, "An error occurred while booking ticket for RouteId: {RouteId} by UserId: {UserId}", bookingRequestDTO.RouteId, bookingRequestDTO.UserId);

                throw new Exception("Error booking ticket.",ex);
            }
        }

        async Task<string> IBookingService.CancelBooking(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Attempting to cancel booking with ID: {BookingId}", id);

                var booking = await _bookingRepository.GetById(id);
                if (booking == null)
                {
                    _logger.LogWarning("Cancellation failed: BookingId {BookingId} not found.", id);

                    throw new NoSuchEntityException();
                }

                // Add a check to prevent canceling an already canceled booking
                if (booking.Status == "Canceled" || booking.Status == "Refunded")
                {
                    _logger.LogWarning("Cancellation failed: BookingId {BookingId} is already in status {Status}.", id, booking.Status);

                    return $"Booking with ID {id} has already been canceled or refunded.";
                }

                // Step 1: Update the booking status
                booking.Status = "Canceled";
                await _bookingRepository.Update(booking.BookingId, booking);

                // Step 2: Create a new refund record
                var newRefund = new Refund
                {
                    BookingId = booking.BookingId,
                    RefundAmount = booking.TotalFare, // Implement a custom refund calculation here if needed
                    Status = "Pending",
                    RefundDate = DateTime.Now
                };
                await _refundRepository.Add(newRefund);

                // Commit the transaction if all steps succeed
                await transaction.CommitAsync();
                _logger.LogInformation("Booking {BookingId} successfully canceled. Refund request created with ID: {RefundId}", id, newRefund.RefundId);

                return $"Booking with ID {id} successfully canceled. A refund request has been created with ID: {newRefund.RefundId}.";
            }
            catch (NoSuchEntityException)
            {
                // If an error occurs, roll back the transaction
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while canceling booking with ID: {BookingId}", id);

                // Log the exception and throw a new, user-friendly exception
                throw new Exception("An error occurred while canceling the booking.", ex);
            }
        }
        

        async Task<string> IBookingService.CancelSelectedSeats(CancelSeatsDTO dto)
        {
            try
            {
                _logger.LogInformation("Attempting to cancel seats for BookingId: {BookingId}", dto.BookingId);

                var booking = await _bookingRepository.GetById(dto.BookingId);
                if (booking == null)
                {
                    _logger.LogWarning("Seat cancellation failed: BookingId {BookingId} not found.", dto.BookingId);

                    throw new NoSuchEntityException();
                }
                var allBookingSeats = await _bookingSeatRepository.GetAll();
                var seatsToCancel = allBookingSeats.Where(bs => bs.BookingId == dto.BookingId && dto.SeatIds.Contains(bs.SeatId)).ToList();

                if (!seatsToCancel.Any())
                {
                    _logger.LogWarning("Seat cancellation failed: No matching seats found for cancellation in BookingId {BookingId}.", dto.BookingId);
                    throw new Exception("No matching seats found for cancellation.");
                }

                foreach (var bookingSeat in seatsToCancel)
                {
                    await _bookingSeatRepository.Delete(bookingSeat.Id);
                }

                
                booking.NoOfSeats -= seatsToCancel.Count;
                booking.TotalFare = booking.TotalFare / (booking.NoOfSeats + seatsToCancel.Count) * booking.NoOfSeats;

                await _bookingRepository.Update(booking.BookingId,booking);
                _logger.LogInformation("Seats for BookingId {BookingId} cancelled successfully.", dto.BookingId);

                return $"Seats for BookingId {dto.BookingId} cancelled successfully.";
            }
            catch(NoSuchEntityException)
            {
                throw;
            }
            catch (Exception)
            {
                _logger.LogError("An error occurred while cancelling seats for BookingId: {BookingId}", dto.BookingId);

                throw new Exception("Error cancelling seats.");
            }
        }

        async Task<IEnumerable<BookingResponseDTO>> IBookingService.GetAllBookings()
        {
            try
            {
                _logger.LogInformation("Fetching all bookings.");

                var bookings = await _bookingRepository.GetAll();
                if (bookings == null || !bookings.Any())
                {
                    _logger.LogWarning("No bookings found in the collection.");

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved all bookings.");

                return _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new Exception("Error getting All Booking",ex);
            }
        }

        async Task<IEnumerable<BookingResponseDTO>> IBookingService.GetBookingById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching booking by ID: {BookingId}", id);

                var booking = await _bookingRepository.GetById(id);
                if (booking == null)
                {
                    _logger.LogWarning("Booking with ID {BookingId} not found.", id);

                    throw new NoSuchEntityException();
                }
                return (IEnumerable<BookingResponseDTO>)_mapper.Map<BookingResponseDTO>(booking);
            }
            catch(NoSuchEntityException)
            {
                throw;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred while getting booking by ID: {BookingId}", id);

                throw new Exception("Error getting Booking by Id");
            }
        }

        async Task<IEnumerable<BookingResponseDTO>> IBookingService.GetBookingsByBusId(int BusId)
        {
            try
            {
                _logger.LogInformation("Fetching bookings for BusId: {BusId}", BusId);

                var bookings = await _bookingRepository.GetBookingsByBusId(BusId);
                if (bookings == null || !bookings.Any())
                {
                    _logger.LogWarning("No bookings found for BusId: {BusId}.", BusId);

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved bookings for BusId: {BusId}", BusId);

                return _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred while getting bookings for BusId: {BusId}", BusId);

                throw new Exception("error getting Booking by bus id");
            }
        }

        async Task<IEnumerable<BookingResponseDTO>> IBookingService.GetBookingsByUser(int UserId)
        {
            try
            {
                _logger.LogInformation("Fetching bookings for UserId: {UserId}", UserId);

                var bookings = await _bookingRepository.GetBookingsByUserId(UserId);
                if (bookings == null || !bookings.Any())
                {
                    _logger.LogWarning("No bookings found for UserId: {UserId}.", UserId);

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved bookings for UserId: {UserId}", UserId);

                return _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);
            }
            catch (NoEntriessInCollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while getting bookings for UserId: {UserId}", UserId);

                throw new Exception("Error getting Bookings by user Id", e);
            }
        }





        async Task<IEnumerable<object>> IBookingService.GetSeatsByBookingId(int BookingId, int UserId)
        {
            try
            {
                _logger.LogInformation("Fetching seats for BookingId: {BookingId} by UserId: {UserId}", BookingId, UserId);

                var seats = await _bookingRepository.GetSeatsByBookingId(BookingId, UserId);
                if (seats == null || !seats.Any())
                {
                    _logger.LogWarning("No seats found for BookingId: {BookingId}.", BookingId);

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved seats for BookingId: {BookingId}", BookingId);

                return seats;
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch (Exception)
            {
                _logger.LogError("An error occurred while fetching seats for BookingId: {BookingId}", BookingId);

                throw new Exception("Error while fetching seats.");
            }

        }
    }
}
