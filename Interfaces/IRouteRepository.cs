using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Route = FastxWebApi.Models.Route;

namespace FastxWebApi.Interfaces
{
    public interface IRouteRepository : IRepository<int, Route>
    {
        Task<PagenationResponseDTO<Route>> SearchRoutes(string origin, string destination, DateTime travelDate,PagenationRequestDTO pagenation);
        Task<PagenationResponseDTO<Route>> GetRoutesByBusId(int busId,PagenationRequestDTO pagenation);
        Task<bool> SoftDelete(int id);



    }
}