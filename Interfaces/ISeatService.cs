
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;


namespace FastxWebApi.Interfaces
{
    public interface ISeatService
    {
        public Task<IEnumerable<SeatDTO>> GetSeatsByRoute(int  routeId);
        public Task<List<SeatDTO>> BookSeats(int routeId,int count);






    }
}
