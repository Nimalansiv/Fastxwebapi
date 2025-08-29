using AutoMapper;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace FastxWebApi.Services
{
    public class SeatService : ISeatService
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

        public async Task<PagenationResponseDTO<SeatDTO>> GetSeatsByRoute(int routeId, PagenationRequestDTO pagination)
        {
            try
            {
                var seatPage = await _seatRepository.GetSeatsByRoute(routeId, pagination);
                if (seatPage == null || !seatPage.Items.Any())
                {
                    throw new NoEntriessInCollectionException();
                }

                var seatDtos = _mapper.Map<IEnumerable<SeatDTO>>(seatPage.Items);
                return new PagenationResponseDTO<SeatDTO>
                {
                    Items = seatDtos,
                    TotalCount = seatPage.TotalCount,
                    PageSize = seatPage.PageSize,
                    CurrentPage = seatPage.CurrentPage
                };
            }
            catch (NoEntriessInCollectionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting seats for RouteId: {RouteId}", routeId);
                throw new Exception("Error getting seats by route", ex);
            }
        }

        public async Task<bool> SoftDeleteSeat(int seatId)
        {
            try
            {
                _logger.LogInformation("Attempting to soft delete seat with ID: {seatId}", seatId);
                var result = await _seatRepository.SoftDelete(seatId);
                if (!result)
                {
                    _logger.LogWarning("Soft delete failed: Seat with ID {seatId} not found or already deleted.", seatId);
                    throw new NoSuchEntityException();
                }

                _logger.LogInformation("Seat with ID {seatId} soft deleted successfully.", seatId);
                return true;
            }
            catch (NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while soft deleting seat with ID: {seatId}", seatId);
                throw new Exception("Error deleting seats.", ex);
            }
        }

        
       

        public async Task<IEnumerable<SeatDTO>> BookSeats(int routeId, List<string> seatNumbers)
        {
            var availableSeats = await _seatRepository.GetAvailableSeatsByRoute(routeId);

            var seatsToBook = availableSeats.Where(s => seatNumbers.Contains(s.SeatNumber)).ToList();

            if (!seatsToBook.Any())
                throw new NoEntriessInCollectionException("No matching seats available to book.");

            if (seatsToBook.Count != seatNumbers.Count)
                throw new Exception("Some requested seats are already booked.");

            foreach (var seat in seatsToBook)
                seat.IsBooked = true;

            await _seatRepository.UpdateRange(seatsToBook);

            return _mapper.Map<IEnumerable<SeatDTO>>(seatsToBook);
        }


    }
}
