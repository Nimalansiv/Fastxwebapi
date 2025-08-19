using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IBusRepository:IRepository<int,Bus>
    {
        Task<PagenationResponseDTO<Bus>> FindAvailableBuses(int routeId, DateTime departureDate, PagenationRequestDTO pagination);
        Task<bool> SoftDelete(int id);

    }
}
