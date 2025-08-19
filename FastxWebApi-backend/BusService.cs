using AutoMapper;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Repository;
using Microsoft.Extensions.Logging;

namespace FastxWebApi.Services
{
    public class BusService:IBusService
    {
        private readonly IBusRepository _busRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BusService> _logger;
        private readonly IUserRepository _userRepository;

        public BusService(IBusRepository busRepository, IMapper mapper, ILogger<BusService> logger, IUserRepository userRepository)
        {
            _busRepository = busRepository;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<BusDTO> AddBus(BusDTO busDTO,int busOperatorId)
        {
            try
            {
                _logger.LogInformation("Attempting to add new bus with name: {BusName} by operator: {BusOperatorId}", busDTO.BusName, busOperatorId);

                var busOperator = await _userRepository.GetById(busOperatorId);
                if (busOperator == null ||( busOperator.RoleId != 3 && busOperator.RoleId != 2)) 
                {
                    _logger.LogWarning("Add bus failed: Bus operator with ID {BusOperatorId} not found or is not a bus operator.", busOperatorId);
                    throw new NoSuchEntityException("Bus operator not found or is not authorized.");
                }

                var bus = _mapper.Map<Bus>(busDTO);
                bus.BusOperatorId = busOperatorId; 

                var addedBus = await _busRepository.Add(bus);
                if (addedBus == null)
                {
                    _logger.LogError("Failed to add bus. Database operation returned null.");
                    throw new Exception("Failed to add bus to the database.");
                }

                _logger.LogInformation("Bus {BusId} added successfully.", addedBus.BusId);
                return _mapper.Map<BusDTO>(addedBus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bus with name: {BusName}", busDTO.BusName);
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<bool> UpdateBus(int id, BusDTO busDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to update bus with ID: {BusId}", id);
                var bus = await _busRepository.GetById(id);
                if (bus == null)
                {
                    _logger.LogWarning("Update failed: BusId {BusId} not found.", id);
                    return false;
                }

                _mapper.Map(busDTO, bus);
                var updatedBus = await _busRepository.Update(bus);
                if (updatedBus == null)
                {
                    _logger.LogError("Update failed for bus with ID: {BusId}.", id);
                    return false;
                }

                _logger.LogInformation("Bus with ID {BusId} updated successfully.", id);
                return true;
            }
            catch (NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating bus with ID: {BusId}", id);
                throw new Exception("Error Updating Bus", ex);
            }
        }

        public async Task<bool> SoftDeleteBus(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to soft delete bus with ID: {id}", id);
                var softDeleteResult = await _busRepository.SoftDelete(id);

                if (!softDeleteResult)
                {
                    _logger.LogWarning("Soft delete failed for bus with ID {id}. Bus not found or already deleted.", id);
                    return false;
                }

                _logger.LogInformation("Bus with ID {id} soft deleted successfully.", id);
                return true;
            }
            catch (NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while soft deleting bus with ID: {BusId}", id);
                throw new Exception($"Error deleting bus with id: {id}.", ex);
            }
        }

        public async Task<PagenationResponseDTO<BusDTO>> GetAllBuses(PagenationRequestDTO pagenation)
        {
            try
            {
                _logger.LogInformation("Fetching paginated list of buses.");
                var buses = await _busRepository.GetAll(pagenation);
                if (buses == null || !buses.Items.Any())
                {
                    _logger.LogWarning("No buses found.");
                    throw new NoEntriessInCollectionException();
                }
                var mappedItems = _mapper.Map<IEnumerable<BusDTO>>(buses.Items);
                var response = new PagenationResponseDTO<BusDTO>
                {
                    Items = mappedItems,
                    TotalCount = buses.TotalCount,
                    PageSize = buses.PageSize,
                    CurrentPage = buses.CurrentPage
                };
                _logger.LogInformation("Successfully fetched paginated buses.");
                return response;
            }
            catch (NoEntriessInCollectionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting paginated buses.");
                throw new Exception("Error getting buses with pagination.", ex);
            }
        }

        public async Task<BusDTO> GetBusById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching bus by ID: {BusId}", id);
                var bus = await _busRepository.GetById(id);
                if (bus == null)
                {
                    _logger.LogWarning("Bus with ID {BusId} not found.", id);
                    throw new NoSuchEntityException();
                }
                return _mapper.Map<BusDTO>(bus);
            }
            catch (NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting bus by ID: {BusId}", id);
                throw new Exception("Error getting bus by id: ", ex);
            }
        }
    }
}
