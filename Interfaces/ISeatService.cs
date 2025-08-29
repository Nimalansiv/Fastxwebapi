using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface ISeatService
    {
        public Task<PagenationResponseDTO<SeatDTO>> GetSeatsByRoute( int routeId,PagenationRequestDTO pagenation);
        public Task<bool> SoftDeleteSeat(int seatId);

        Task<IEnumerable<SeatDTO>> BookSeats(int routeId, List<string> seatNumbers);


    }
}
