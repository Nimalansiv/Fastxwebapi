
using FastxWebApi.Models;
using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;


namespace FastxWebApi.Interfaces
{
    public interface IBookingService
    {
        public Task< string> BookTicket(BookingRequestDTO bookingRequestDTO);

        public Task<IEnumerable<BookingResponseDTO>> GetAllBookings();

        public Task<string> CancelBooking(int id);

        public Task<IEnumerable<BookingResponseDTO>> GetBookingById(int id);

        public Task<string> CancelSelectedSeats(CancelSeatsDTO dto);

        public Task<IEnumerable<object>> GetSeatsByBookingId(int BookingId, int UserId);

        public Task<IEnumerable<BookingResponseDTO>> GetBookingsByBusId(int BusId);

        public Task<IEnumerable<BookingResponseDTO>> GetBookingsByUser(int UserId);
        












    }
}
