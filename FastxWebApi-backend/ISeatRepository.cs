using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface ISeatRepository:IRepository<int,Seat>
    {
        public Task<PagenationResponseDTO<Seat>> GetSeatsByRoute(int routeId, PagenationRequestDTO pagenation);
        public Task<IEnumerable<Seat>> UpdateAll(IEnumerable<Seat> seats);
        Task<bool> SoftDelete(int id);

    }
}
