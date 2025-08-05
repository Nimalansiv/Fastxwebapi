using FastxWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Route = FastxWebApi.Models.Route;

namespace FastxWebApi.Interfaces
{
    public interface IRouteRepository : IRepository<int, Route>
    {
        Task<IEnumerable<Route>> SearchRoutes(string origin, string destination, DateTime travelDate);
        Task<IEnumerable<Route>> GetRoutesByBusId(int busId);
    }
}