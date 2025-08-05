using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IRouteService
    {
        public Task<IEnumerable<RouteDTO>> GetAllRoutes();

        public Task<RouteDTO> GetRouteById(int id);
        public Task<string> AddRoute(RouteDTO routeDTO,int busOperatorId);

        public Task<string> UpdateRoute(int id, RouteDTO routeDTO, int busOperatorId);

        public Task<string> DeleteRoute(int id, int busOperatorId);

        public Task<IEnumerable<RouteDTO>> SearchRoutes(string origin, string destination,DateTime travelDate);

        public Task<IEnumerable<RouteDTO>> GetRoutesByBusId(int busId);

    }
}
