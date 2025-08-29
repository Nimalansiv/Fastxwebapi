using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Route = FastxWebApi.Models.Route;

namespace FastxWebApi.Services
{
    public class RouteService : IRouteService
    {

        private readonly IRouteRepository _routeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBusRepository _busRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RouteService> _logger;
        private readonly ApplicationDbContext _context;

        public RouteService(IRouteRepository routeRepository, IUserRepository userRepository, IBusRepository busRepository, IMapper mapper, ILogger<RouteService> logger, ApplicationDbContext context)
        {
            _routeRepository = routeRepository;
            _userRepository = userRepository;
            _busRepository = busRepository;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<RouteDTO> AddRoute(RouteDTO routeDTO, int busOperatorId)
        {
            _logger.LogInformation("BusOperatorId received in service: {busOperatorId}", busOperatorId);
            try
            {
                _logger.LogInformation("Attempting to add route for BusId: {BusId} by operator {OperatorId}", routeDTO.BusId, busOperatorId);

                var operatorBus = await _context.Buses.Include(b => b.BusOperator).SingleOrDefaultAsync(b => b.BusId == routeDTO.BusId);
                if (operatorBus == null)
                {
                    _logger.LogWarning("Add route failed: Bus with ID {BusId} does not exist.", routeDTO.BusId);
                    throw new NoSuchEntityException($"Bus with ID {routeDTO.BusId} does not exist.");
                }

                var busOperator = await _userRepository.GetById(busOperatorId);

                if (busOperator.RoleId != 3 && busOperator.RoleId != 2) 
                {
                    _logger.LogWarning("Add route failed: User with ID {busOperatorId} is not an admin.", busOperatorId);
                    throw new UnauthorizedAccessException("User not authorized to add route.");
                }

                

                var route = _mapper.Map<Route>(routeDTO);
                var addedRoute = await _routeRepository.Add(route);

                if (addedRoute == null)
                {
                    _logger.LogError("Failed to add route. Database operation returned null.");
                    throw new Exception("Failed to add route to the database.");
                }

                _logger.LogInformation("Route with id {RouteId} added successfully.", addedRoute.RouteId);
                return _mapper.Map<RouteDTO>(addedRoute);
            }
            catch (UnauthorizedAccessException) 
            { 
                throw;
            }
            catch (NoSuchEntityException) 
            {
                throw; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding route.");
                throw new Exception("Error adding route.", ex);
            }
        }

        
        public async Task<bool> UpdateRoute(int id, RouteDTO routeDTO, int busOperatorId)
        {
            try
            {
                _logger.LogInformation("Attempting to update route with ID: {id}", id);

                var user = await _userRepository.GetById(busOperatorId); 
                var existingRoute = await _routeRepository.GetById(id); 

                if (existingRoute == null)
                {
                    _logger.LogWarning("Update failed: Route with ID {id} not found.", id);
                    return false;
                }

               
                if (user?.Role?.RoleName != "Admin" && user?.Role?.RoleName != "Bus Operator")
                {
                    _logger.LogWarning("Update failed: Operator {OperatorId} is not authorized for RouteId {id}.", busOperatorId, id);
                    throw new UnauthorizedAccessException("You are not authorized to update this route.");
                }

                _mapper.Map(routeDTO, existingRoute);
                await _routeRepository.Update(existingRoute);

                _logger.LogInformation("Route with id {id} updated successfully.", id);
                return true;
            }
            catch (UnauthorizedAccessException) 
            { 
                throw;
            }
            catch (NoSuchEntityException) 
            { 
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating route with ID: {id}", id);
                throw new Exception("Error updating route", ex);
            }
        }

     
        public async Task<bool> SoftDeleteRoute(int id, int busOperatorId)
        {
            try
            {
                _logger.LogInformation("Attempting to soft delete route with ID: {id}", id);
                var user = await _userRepository.GetById(busOperatorId);
                var existingRoute = await _routeRepository.GetById(id);

                if (existingRoute == null)
                {
                    _logger.LogWarning("Soft delete failed: Route with ID {id} not found.", id);
                    return false;
                }

                if (user?.Role?.RoleName != "Admin" && existingRoute.Bus.BusOperator?.UserId != busOperatorId)
                {
                    _logger.LogWarning("Soft delete failed: Operator {OperatorId} is not authorized for RouteId {id}.", busOperatorId, id);
                    throw new UnauthorizedAccessException("You are not authorized to delete this route.");
                }

                var softDeleteResult = await _routeRepository.SoftDelete(id);

                if (!softDeleteResult)
                {
                    _logger.LogError("Failed to soft delete route {id} in the repository.", id);
                    return false;
                }

                _logger.LogInformation("Route with ID {id} soft deleted successfully.", id);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting route");
                throw new Exception($"Error deleting route with id: {id}.", ex);
            }
        }

        public async Task<PagenationResponseDTO<RouteDTO>> GetAllRoutes(PagenationRequestDTO pagenation)
        {
            try
            {
                _logger.LogInformation("Fetching all routes with pagination.");
                var routesPaged = await _routeRepository.GetAll(pagenation);
                if (routesPaged == null || !routesPaged.Items.Any())
                {
                    _logger.LogWarning("No routes found in the collection.");
                    throw new NoEntriessInCollectionException();
                }

                var mappedRoutes = _mapper.Map<IEnumerable<RouteDTO>>(routesPaged.Items);
                return new PagenationResponseDTO<RouteDTO>
                {
                    Items = mappedRoutes,
                    TotalCount = routesPaged.TotalCount,
                    CurrentPage = pagenation.PageNumber,
                    PageSize = pagenation.PageSize
                };
            }
            catch (NoEntriessInCollectionException) 
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all routes.");
                throw new Exception("Error fetching all routes", ex);
            }
        }

        public async Task<RouteDTO> GetRouteById(int id)
        {
            try
            {
                var route = await _routeRepository.GetById(id);
                if (route == null)
                {
                    _logger.LogWarning("Route with ID {id} not found.", id);
                    throw new NoSuchEntityException();
                }

                return _mapper.Map<RouteDTO>(route);
            }
            catch (NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting route by ID.");
                throw new Exception("Error getting route by id", ex);
            }
        }

        public async Task<PagenationResponseDTO<RouteDTO>> GetRoutesByBusId(int busId, PagenationRequestDTO pagenation)
        {
            try
            {
                _logger.LogInformation("Fetching routes for BusId: {BusId}", busId);
                var routesPaged = await _routeRepository.GetRoutesByBusId(busId, pagenation);

                if (routesPaged == null || !routesPaged.Items.Any())
                {
                    _logger.LogWarning("No routes found for BusId: {BusId}.", busId);
                    throw new NoEntriessInCollectionException();
                }

                var mappedRoutes = _mapper.Map<IEnumerable<RouteDTO>>(routesPaged.Items);
                return new PagenationResponseDTO<RouteDTO>
                {
                    Items = mappedRoutes,
                    TotalCount = routesPaged.TotalCount,
                    CurrentPage = pagenation.PageNumber,
                    PageSize = pagenation.PageSize
                };
            }
            catch (NoEntriessInCollectionException)
            { 
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting routes by bus ID");
                throw new Exception("Error getting routes by bus id", ex);
            }
        }

        public async Task<PagenationResponseDTO<RouteDTO>> SearchRoutes(string origin, string destination, DateTime travelDate, PagenationRequestDTO pagenation)
        {
            try
            {
                _logger.LogInformation("Searching routes with filters and pagination.");
                var routePagedResult = await _routeRepository.SearchRoutes(origin, destination, travelDate, pagenation);

                if (routePagedResult.Items == null || !routePagedResult.Items.Any())
                {
                    throw new NoEntriessInCollectionException();
                }

                var mappedRoutes = _mapper.Map<IEnumerable<RouteDTO>>(routePagedResult.Items);

                return new PagenationResponseDTO<RouteDTO>
                {
                    Items = mappedRoutes,
                    TotalCount = routePagedResult.TotalCount,
                    CurrentPage = routePagedResult.CurrentPage,
                    PageSize = routePagedResult.PageSize
                };
            }
            catch (NoEntriessInCollectionException) 
            { 
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while searching routes.");
                throw new Exception("Error while searching routes.", ex);
            }
        }
    }
}
