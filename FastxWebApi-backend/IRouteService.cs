using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IRouteService
    {
        public Task<PagenationResponseDTO<RouteDTO>> GetAllRoutes(PagenationRequestDTO pagenation);

        public Task<RouteDTO> GetRouteById(int id);
        public Task<RouteDTO> AddRoute(RouteDTO routeDTO,int busOperatorId);

        public Task<bool> UpdateRoute(int id, RouteDTO routeDTO, int busOperatorId);

        public Task<bool> SoftDeleteRoute(int id, int busOperatorId);

        public Task<PagenationResponseDTO<RouteDTO>> SearchRoutes(string origin, string destination,DateTime travelDate,PagenationRequestDTO pagenation);

        public Task<PagenationResponseDTO<RouteDTO>> GetRoutesByBusId(int busId,PagenationRequestDTO pagenation);

    }
}
