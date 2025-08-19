using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IBookingRepository:IRepository<int,Booking>
    {
       public Task<PagenationResponseDTO<Booking>> GetBookingsByUserId(int userId, PagenationRequestDTO pagination);

       public Task<PagenationResponseDTO<Booking>> GetBookingsByBusId(int busId,PagenationRequestDTO pagination);

        public Task<IEnumerable<Seat>> GetSeatsByBookingId(int bookingId, int userId);

        public Task<bool> SoftDelete(int id);


    }
}
