using FastxWebApi.Models;

namespace FastxWebApi.Interfaces
{
    public interface IBookingRepository:IRepository<int,Booking>
    {
       public Task<IEnumerable<Booking>> GetBookingsByUserId(int userId);

       public Task<IEnumerable<Booking>> GetBookingsByBusId(int busId);

        public Task<IEnumerable<object>> GetSeatsByBookingId(int bookingId, int userId);

    }
}
