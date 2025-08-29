using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IBusService
    {
        public Task<PagenationResponseDTO<BusDTO>> GetAllBuses(PagenationRequestDTO pagenation);
        public Task<BusDTO> GetBusById(int id);

        public Task<BusDTO> AddBus(BusDTO busDTO, int busOperatorId);

        public Task<bool> UpdateBus(int id, BusDTO busDTO);

        public Task<bool> SoftDeleteBus(int id);

    }
}
