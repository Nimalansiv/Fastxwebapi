


using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IBusService
    {
        public Task<IEnumerable<BusDTO>> GetAllBuses();
        public Task<BusDTO> GetBusById(int id);

        public Task<string> AddBus(BusDTO busDTO);

        public Task<string> UpdateBus(int id, BusDTO busDTO);

        public Task<string> DeleteBus(int id);

    }
}
