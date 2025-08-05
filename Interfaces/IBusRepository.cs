using FastxWebApi.Models;

namespace FastxWebApi.Interfaces
{
    public interface IBusRepository:IRepository<int,Bus>
    {
        public Task<IEnumerable<Bus>> GetAvailableBuses();
    }
}
