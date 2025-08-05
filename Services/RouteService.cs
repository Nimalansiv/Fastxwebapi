using AutoMapper;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using Route = FastxWebApi.Models.Route;
using FastxWebApi.Models;
using Microsoft.Extensions.Logging;
using log4net.Repository.Hierarchy;


namespace FastxWebApi.Services
{
    public class RouteService:IRouteService
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBusRepository _busRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RouteService> _logger;


        public RouteService(IRouteRepository routeRepository, IUserRepository userRepository, IBusRepository busRepository, IMapper mapper, ILogger<RouteService> logger)
        {
            _routeRepository = routeRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _busRepository = busRepository;
            _logger = logger;
        }

        public async Task<string> AddRoute(RouteDTO routeDTO, int busOperatorId)
        {
            try
            {
                _logger.LogInformation("Attempting to add route for BusId: {BusId} by operator {OperatorId}", routeDTO.BusId, busOperatorId);

                var operatorBus = await _busRepository.GetById(routeDTO.BusId);
                var busOperator = await _userRepository.GetById(busOperatorId);

                if (operatorBus == null || busOperator == null || operatorBus.BusOperator?.UserId != busOperatorId)
                {
                    _logger.LogWarning("Unauthorized attempt to add route. Bus or operator not found, or ownership mismatch.");

                    throw new UnauthorizedAccessException("You are not authorized to add a route for this bus.");
                }
                var route = _mapper.Map<Route>(routeDTO);
                var addedRoute = await _routeRepository.Add(route);
                if (addedRoute == null)
                {
                    _logger.LogError("Failed to add route. Database operation returned null.");

                    throw new Exception("Failed to add route to the database.");
                }
                _logger.LogInformation("Route {RouteId} added successfully.", addedRoute.RouteId);

                return $"Route with id {addedRoute.RouteId} added successfully.";
            }
            catch(UnauthorizedAccessException)
            {
                throw;
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding route for BusId: {BusId} by operator {OperatorId}", routeDTO.BusId, busOperatorId);

                throw new Exception("error adding route");
            }
        }

        public async Task<string> DeleteRoute(int id, int busOperatorId)
        {
            try
            {
                _logger.LogInformation("Attempting to delete route with ID: {RouteId} by operator {OperatorId}", id, busOperatorId);

                var existingRoute = await _routeRepository.GetById(id);
                if (existingRoute == null)
                {
                    _logger.LogWarning("Deletion failed: RouteId {RouteId} not found.", id);

                    throw new NoSuchEntityException();
                }
                if (existingRoute.Bus.BusOperator?.UserId != busOperatorId)
                {
                    _logger.LogWarning("Unauthorized attempt to delete route {RouteId}. Ownership mismatch.", id);

                    throw new UnauthorizedAccessException("You are not authorized to delete this route.");
                }
                await _routeRepository.Delete(id);
                _logger.LogInformation("Route with ID {RouteId} deleted successfully.", id);

                return $"Route with id {id} deleted successfully.";
            }
            catch(UnauthorizedAccessException)
            {
                throw;
            }
            catch (NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting route with ID: {RouteId} by operator {OperatorId}", id, busOperatorId);

                throw new Exception($"Error deleting route with id: {id}. {ex.Message}");
            }
        }

        public async Task<IEnumerable<RouteDTO>> GetAllRoutes()
        {
            try
            {
                _logger.LogInformation("Fetching all routes.");
                var routes = await _routeRepository.GetAll();
                if (routes == null || !routes.Any())
                {
                    _logger.LogWarning("No routes found in the collection.");

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved all routes.");

                return _mapper.Map<IEnumerable<RouteDTO>>(routes);
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all routes.");

                throw new Exception("Error getting all routes");
            }
        }

        public async Task<RouteDTO> GetRouteById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching route by ID: {RouteId}", id);

                var route = await _routeRepository.GetById(id);
                if (route == null)
                {
                    _logger.LogWarning("Route with ID {RouteId} not found.", id);

                    throw new NoSuchEntityException();
                }
                _logger.LogInformation("Successfully retrieved route with ID: {RouteId}", id);

                return _mapper.Map<RouteDTO>(route);
            }
            catch(NoSuchEntityException)
            {
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting route by ID: {RouteId}", id);

                throw new Exception("Error getting route by id");
            }
        }

        public async Task<IEnumerable<RouteDTO>> GetRoutesByBusId(int busId)
        {
            try
            {
                _logger.LogInformation("Fetching routes for BusId: {BusId}", busId);

                var routes = await _routeRepository.GetRoutesByBusId(busId);
                if (routes == null || !routes.Any())
                {
                    _logger.LogWarning("No routes found for BusId: {BusId}.", busId);

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved routes for BusId: {BusId}", busId);

                return _mapper.Map<IEnumerable<RouteDTO>>(routes);
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting routes for BusId: {BusId}", busId);
                throw new Exception("Error getting routes by bus id.");
            }
        }

        public async Task<IEnumerable<RouteDTO>> SearchRoutes(string origin, string destination, DateTime travelDate)
        {
            try
            {
                _logger.LogInformation("Searching for routes from {Origin} to {Destination} on {TravelDate}", origin, destination, travelDate.ToShortDateString());

                var routes = await _routeRepository.SearchRoutes(origin, destination, travelDate);
                if (routes == null || !routes.Any())
                {
                    _logger.LogWarning("No routes found for search criteria: from {Origin} to {Destination} on {TravelDate}", origin, destination, travelDate.ToShortDateString());

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully found {Count} routes for search criteria.", routes.Count());

                return _mapper.Map<IEnumerable<RouteDTO>>(routes);
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for routes.");

                throw new Exception("Error searching for routes.");
            }

        }

        public async Task<string> UpdateRoute(int id, RouteDTO routeDTO, int busOperatorId)
        {
            try
            {
                _logger.LogInformation("Attempting to update route with ID: {RouteId} by operator {OperatorId}", id, busOperatorId);

                var existingRoute = await _routeRepository.GetById(id);
                if (existingRoute == null)
                {
                    _logger.LogWarning("Update failed: RouteId {RouteId} not found.", id);

                    throw new NoSuchEntityException();
                }
                if (existingRoute.BusId != routeDTO.BusId || existingRoute.Bus.BusOperator?.UserId != busOperatorId)
                {
                    _logger.LogWarning("Unauthorized attempt to update route {RouteId}. Ownership mismatch.", id);

                    throw new UnauthorizedAccessException("You are not authorized to update this route.");
                }
               
                _mapper.Map(routeDTO, existingRoute);

                await _routeRepository.Update(id, existingRoute);
                _logger.LogInformation("Route with ID {RouteId} updated successfully.", id);

                return $"Route with id {id} updated successfully.";
            }
            catch(UnauthorizedAccessException)
            {
                throw;
            }
            catch(NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating route with ID: {RouteId} by operator {OperatorId}", id, busOperatorId);

                throw new Exception("Error updating route.");
            }
        }
    }
}
