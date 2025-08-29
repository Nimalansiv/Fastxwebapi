using FastxWebApi.Models;
using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;


namespace FastxWebApi.Interfaces
{
    public interface IBookingService
    {
        public Task< string> BookTicket(BookingRequestDTO bookingRequestDTO);

        public Task<PagenationResponseDTO<BookingResponseDTO>> GetAllBookings(PagenationRequestDTO pagination);

        public Task<string> CancelBooking(int id);

        public Task<BookingResponseDTO> GetBookingById(int id);

        public Task<string> CancelSelectedSeats(CancelSeatsDTO dto);

        public Task<IEnumerable<SeatDTO>> GetSeatsByBookingId(int BookingId, int UserId);

        public Task<PagenationResponseDTO<BookingResponseDTO>> GetBookingsByBusId(int BusId, PagenationRequestDTO pagination);

        public Task<PagenationResponseDTO<BookingResponseDTO>> GetBookingsByUser(int UserId, PagenationRequestDTO pagination);


    }
}
