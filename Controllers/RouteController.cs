using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FastxWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("DefaultCORS")]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;
        private readonly ILogger<RouteController> _logger;

        public RouteController(IRouteService routeService, ILogger<RouteController> logger)
        {
            _routeService = routeService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Bus Operator")]
        public async Task<IActionResult> AddRoute(RouteDTO routeDTO)
        {
            var busOperatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _logger.LogInformation("BusOperatorId received: {busOperatorId}", busOperatorId);
            try
            {
                _logger.LogInformation("Attempting to add route for BusId: {BusId} by operator {OperatorId}", routeDTO.BusId, busOperatorId);
                var result = await _routeService.AddRoute(routeDTO, busOperatorId);
                return CreatedAtAction(nameof(GetRouteById), new { id = result.RouteId }, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized attempt to add route for BusId: {BusId}", routeDTO.BusId);
                return Unauthorized(ex.Message);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Bus or operator not found during route creation.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new route.");
                return BadRequest(ex.Message);
            }
        }

      
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Bus Operator")]
        public async Task<IActionResult> SoftDeleteRoute(int id)
        {
            var busOperatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _logger.LogInformation("BusOperatorId received: {busOperatorId}", busOperatorId);
            try
            {
                var result = await _routeService.SoftDeleteRoute(id, busOperatorId);
                if (result)
                {
                    return NoContent();
                }
                return NotFound($"Route with ID {id} not found or already deleted.");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized attempt to delete route with ID: {Id}", id);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting route with ID: {Id}.", id);
                return StatusCode(500, ex.Message);
            }
        }

       
        [HttpGet]
        [Authorize(Roles = "Admin,User,Bus Operator")]
        public async Task<ActionResult<PagenationResponseDTO<RouteDTO>>> GetAllRoutes([FromQuery] PagenationRequestDTO pagenation)
        {
            try
            {
                var routes = await _routeService.GetAllRoutes(pagenation);
                return Ok(routes);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No routes found in the database.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting all routes.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User,Bus Operator")]
        public async Task<ActionResult<RouteDTO>> GetRouteById(int id)
        {
            try
            {
                var route = await _routeService.GetRouteById(id);
                return Ok(route);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Route with ID {Id} was not found.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting route with ID {Id}.", id);
                return StatusCode(500, ex.Message);
            }
        }

        
        [HttpGet("GetRoutesByBusId/{busId}")]
        [Authorize(Roles = "Admin,Bus Operator")]
        public async Task<ActionResult<PagenationResponseDTO<RouteDTO>>> GetRoutesByBusId(int busId, PagenationRequestDTO pagenation)
        {
            try
            {
                var routes = await _routeService.GetRoutesByBusId(busId, pagenation);
                return Ok(routes);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No routes found for BusId: {BusId}.", busId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting routes for BusId: {BusId}.", busId);
                return StatusCode(500, ex.Message);
            }
        }

        
        [HttpGet("search_Routes")]
        [Authorize(Roles = "User,Admin,Bus Operator")]
        public async Task<ActionResult<PagenationResponseDTO<RouteDTO>>> SearchRoutes( [FromQuery] string origin, [FromQuery] string destination, DateTime travelDate,  [FromQuery]PagenationRequestDTO pagenation)
        {
            try
            {
                var routes = await _routeService.SearchRoutes(origin, destination, travelDate, pagenation);
                return Ok(routes);
            }
            catch (NoEntriessInCollectionException ex)
            {
                _logger.LogWarning(ex, "No routes found for search criteria.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for routes.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Bus Operator")]
        public async Task<IActionResult> UpdateRoute(int id, [FromBody]RouteDTO routeDTO, int busOperatorId)
        {
            if (id != routeDTO.RouteId)
            {
                return BadRequest("Route ID in the URL does not match the ID in the request body.");
            }
            try
            {
                busOperatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _routeService.UpdateRoute(id,routeDTO, busOperatorId);
                if (result)
                {
                    return NoContent();
                }
                return NotFound($"Route with ID {id} not found.");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized attempt to update route with ID: {Id}", id);
                return Unauthorized(ex.Message);
            }
            catch (NoSuchEntityException ex)
            {
                _logger.LogWarning(ex, "Update failed: RouteId {Id} not found.", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating route with ID: {Id}.", id);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
