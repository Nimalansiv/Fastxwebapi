using AutoMapper;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace FastxWebApi.Services
{
    public class SeatService:ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SeatService> _logger;

        public SeatService(ISeatRepository seatRepository, IMapper mapper, ILogger<SeatService> logger)
        {
            _seatRepository = seatRepository;
            _mapper = mapper;
            _logger = logger;
        }

        async Task<List<SeatDTO>> ISeatService.BookSeats(int routeId, int count)
        {
            try
            {
                _logger.LogInformation("Attempting to get {Count} seats for RouteId: {RouteId}", count, routeId);

                var seats = await _seatRepository.GetSeatsByRoute(routeId);
                if (seats == null || seats.Count() < count)
                {
                    _logger.LogWarning("Not enough seats available on RouteId {RouteId}. Requested: {Count}, Available: {AvailableSeats}", routeId, count, seats?.Count() ?? 0);

                    throw new Exception("Not enough seats available on this route.");
                }

                var seatsToBook = seats.Take(count).ToList();
                _logger.LogInformation("Successfully retrieved {Count} seats for RouteId: {RouteId}", count, routeId);
                return _mapper.Map<List<SeatDTO>>(seatsToBook);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to get seats for RouteId: {RouteId}", routeId);

                throw new Exception("Error booking seats.");
            }
        }

        async Task<IEnumerable<SeatDTO>> ISeatService.GetSeatsByRoute(int routeId)
        {
            try
            {
                _logger.LogInformation("Fetching seats for RouteId: {RouteId}", routeId);
                var seats = await _seatRepository.GetSeatsByRoute(routeId);
                if (seats == null || !seats.Any())
                {
                    _logger.LogWarning("No seats found for RouteId: {RouteId}.", routeId);
                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved seats for RouteId: {RouteId}", routeId);
                return _mapper.Map<IEnumerable<SeatDTO>>(seats);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting seats for RouteId: {RouteId}", routeId);
                throw new Exception("Error getting seats by route");
            }
        }
    }
}
