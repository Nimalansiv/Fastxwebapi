using AutoMapper;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace FastxWebApi.Services
{
    public class BusService:IBusService
    {
        private readonly IBusRepository _busRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BusService> _logger;


        public BusService(IBusRepository busRepository, IMapper mapper, ILogger<BusService> logger)
        {
            _busRepository = busRepository;
            _mapper = mapper;
            _logger = logger;
        }   

        async Task<string> IBusService.AddBus(BusDTO busDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to add new bus with name: {BusName}", busDTO.BusName);

                var bus = _mapper.Map<Bus>(busDTO);
                var addedBus = await _busRepository.Add(bus);
                if (addedBus == null)
                {
                    _logger.LogError("Failed to add bus. Database operation returned null.");

                    throw new Exception("Failed to add bus to the database.");
                }
                _logger.LogInformation("Bus {BusId} added successfully.", addedBus.BusId);

                return $"Bus with id {addedBus.BusId} added successfully.";
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding bus with name: {BusName}", busDTO.BusName);

                throw new Exception("Error adding Bus");
            }
        }

        async Task <string> IBusService.DeleteBus(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete bus with ID: {BusId}", id);

                var deletedBus = await _busRepository.Delete(id);
                if (deletedBus == null)
                {
                    _logger.LogWarning("Deletion failed: BusId {BusId} not found.", id);

                    throw new NoSuchEntityException();
                }
                _logger.LogInformation("Bus with ID {BusId} deleted successfully.", id);

                return $"Bus with ID {id} deleted successfully.";
            }
            catch (NoSuchEntityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bus with ID: {BusId}", id);

                throw new Exception($"Error deleting bus with id: {id}. {ex.Message}");
            }
        }

        async Task<IEnumerable<BusDTO>> IBusService.GetAllBuses()
       {
            try
            {
                _logger.LogInformation("Fetching all buses.");

                var buses = await _busRepository.GetAll();
                if (buses == null || !buses.Any())
                {
                    _logger.LogWarning("No buses found in the collection.");

                    throw new NoEntriessInCollectionException();
                }
                _logger.LogInformation("Successfully retrieved all buses.");

                return _mapper.Map<IEnumerable<BusDTO>>(buses);
            }
            catch(NoEntriessInCollectionException)
            {
                throw;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred while getting all buses.");

                throw new Exception("Error getting all buses");
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

                throw new Exception("Error getting bus by id: ");
            }
        }

         async Task<string> IBusService.UpdateBus(int id, BusDTO busDTO)
         {
            try
            {
                _logger.LogInformation("Attempting to update bus with ID: {BusId}", id);

                var bus = await _busRepository.GetById(id);
                if (bus == null)
                {
                    _logger.LogWarning("Update failed: BusId {BusId} not found.", id);

                    throw new NoSuchEntityException();
                }

                _mapper.Map(busDTO, bus);
                await _busRepository.Update(id,bus);
                _logger.LogInformation("Bus with ID {BusId} updated successfully.", id);

                return $"Bus with id {id} updated successfully.";
            }
            catch(NoSuchEntityException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while updating bus with ID: {BusId}", id);

                throw new Exception("Error Updating Bus",e);
            }
         }
    }
}
