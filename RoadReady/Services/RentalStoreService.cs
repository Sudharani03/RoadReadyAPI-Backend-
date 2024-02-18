using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;
using RoadReady.Repositories;

namespace RoadReady.Services
{
    public class RentalStoreService : IRentalStoreAdminService, IRentalStoreUserService
    {
        private readonly IRepository<int, RentalStore> _rentalStoreRepository;
        private readonly ILogger<RentalStoreService> _logger;
        public RentalStoreService(IRepository<int, RentalStore> rentalStoreRepository, ILogger<RentalStoreService> logger)
        {
            _rentalStoreRepository = rentalStoreRepository;
            _logger = logger;
        }

        #region --> AddRentalStore
        public async Task<RentalStore> AddRentalStore(RentalStore rentalStore)
        {
            try
            {
                return await _rentalStoreRepository.Add(rentalStore);
            }
            catch (RentalStoreAlreadyExistsException ex)
            {
                _logger.LogWarning($"Rental store with ID {rentalStore.StoreId} already exists.");
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding the rental store: {ex.Message}");
                throw; // Re-throw the exception
            }
        }
        #endregion

        #region --> RemoveRentalStore
        public async Task<RentalStore> RemoveRentalStore(int id)
        {
            try
            {
                return await _rentalStoreRepository.Delete(id);
            }
            catch (NoSuchRentalStoreException ex)
            {
                _logger.LogWarning($"Rental store with ID {id} not found.");
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting the rental store: {ex.Message}");
                throw; // Re-throw the exception
            }
        }
        #endregion

        #region -->GetAllRentalStores
        public async Task<List<RentalStore>> GetAllRentalStores()
        {
            try
            {
                return await _rentalStoreRepository.GetAsync();
            }
            catch (RentalStoreListEmptyException ex)
            {
                _logger.LogInformation("Rental store list is empty.");
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting all rental stores: {ex.Message}");
                throw; // Re-throw the exception
            }
        }
        #endregion

        #region --> GetRentalStoreById
        public async Task<RentalStore> GetRentalStoreById(int storeId)
        {
            try
            {
                var rentalStore = await _rentalStoreRepository.GetAsyncById(storeId);

                if (rentalStore != null)
                {
                    return rentalStore;
                }
                else
                {
                    _logger.LogInformation($"Rental store with ID {storeId} not found.");
                    throw new NoSuchRentalStoreException();
                }
            }
            catch (NoSuchRentalStoreException ex)
            {
                _logger.LogInformation($"No such rental store found: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting rental store by ID: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> GetCarsInStore
        public async Task<List<CarStore>> GetCarsInStore(int storeId)
        {
            try
            {
                var rentalStore = await _rentalStoreRepository.GetAsyncById(storeId);

                if (rentalStore != null && rentalStore.CarStore != null)
                {
                    return rentalStore.CarStore.ToList();
                }
                else
                {
                    _logger.LogInformation($"No cars found in rental store with ID {storeId}.");
                    throw new NoSuchCarException();
                }
            }
            catch (NoSuchCarException ex)
            {
                _logger.LogInformation($"No cars found in the rental store: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting cars in the rental store: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> UpdateRentalStoreDetails
        public async Task<RentalStore> UpdateRentalStoreDetails(int storeId, string pickUpLocation, string dropOffLocation)
        {
            try
            {
                RentalStore rentalStore = await _rentalStoreRepository.GetAsyncById(storeId);

                if (rentalStore == null)
                {
                    _logger.LogWarning($"User with ID {storeId} not found.");
                    throw new NoSuchRentalStoreException();
                }

                rentalStore.PickUpLocation = pickUpLocation;

                await _rentalStoreRepository.Update(rentalStore);

                return rentalStore;
            }
            catch (NoSuchRentalStoreException ex)
            {
                _logger.LogWarning($"Rental store with ID {storeId} not found.");
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating the rental store details: {ex.Message}");
                throw; // Re-throw the exception
            }
        }
        #endregion
    }
}
