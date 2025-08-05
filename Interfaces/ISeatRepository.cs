using FastxWebApi.Models;

namespace FastxWebApi.Interfaces
{
    public interface ISeatRepository:IRepository<int,Seat>
    {

        public Task<IEnumerable<Seat>> GetSeatsByRoute(int routeId);



    }
}
