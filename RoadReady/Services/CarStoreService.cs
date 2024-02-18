using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Services
{
    public class CarStoreService : ICarStoreAdminServices, ICarStoreUserServices
    {
        private readonly IRepository<int, CarStore> _carStoreRepository;
        private readonly IRepository<int, Car> _carRepository;
        private readonly IRepository<int, RentalStore> _rentalStoreRepository;
        private readonly ILogger<CarStoreService> _logger;

        public CarStoreService(IRepository<int, CarStore> carStoreRepository,IRepository<int, Car> carRepository,IRepository<int, RentalStore> rentalStoreRepository,ILogger<CarStoreService> logger)
        {
            _carStoreRepository = carStoreRepository;
            _carRepository = carRepository;
            _rentalStoreRepository = rentalStoreRepository;
            _logger = logger;
        }

        #region --> AddCarToStore
        public async Task<CarStore> AddCarToStore(int storeId, int carId)
        {
            try
            {
                // Check if the car exists
                var car = await _carRepository.GetAsyncById(carId) ?? throw new NoSuchCarException();

                // Check if the store exists
                var store = await _rentalStoreRepository.GetAsyncById(storeId) ?? throw new NoSuchRentalStoreException();

                // Check if the car is already associated with the store
                if (store.CarStore?.Any(cs => cs.CarId == carId) == true)
                {
                    _logger.LogWarning($"Car with ID {carId} is already associated with store {storeId}");
                    throw new CarAlreadyExistsException();
                }

                // Create CarStore entry
                var carStore = new CarStore
                {
                    CarId = carId,
                    StoreId = storeId
                };

                // Add the CarStore entry to the repository
                await _carStoreRepository.Add(carStore);

                return carStore;
            }
            catch (NoSuchCarException ex)
            {
                _logger.LogError($"Error in AddCarToStore: {ex.Message}");
                throw;
            }
            catch (NoSuchRentalStoreException ex)
            {
                _logger.LogError($"Error in AddCarToStore: {ex.Message}");
                throw;
            }
            catch (CarAlreadyExistsException ex)
            {
                _logger.LogError($"Error in AddCarToStore: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddCarToStore: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region --> RemoveCarFromStore
        public async Task<CarStore> RemoveCarFromStore(int storeId, int carId)
        {
            try
            {
                // Check if the store exists
                var store = await _rentalStoreRepository.GetAsyncById(storeId) ?? throw new NoSuchRentalStoreException();

                // Check if the CarStore entry exists
                var carStore = store.CarStore?.FirstOrDefault(cs => cs.CarId == carId);
                if (carStore == null)
                {
                    _logger.LogWarning($"Car with ID {carId} is not associated with store {storeId}");
                    throw new NoSuchCarException();
                }

                // Remove the CarStore entry from the repository
                await _carStoreRepository.Delete(carStore.StoreId);

                return carStore;
            }
            catch (NoSuchRentalStoreException ex)
            {
                _logger.LogError($"Error in RemoveCarFromStore: {ex.Message}");
                throw;
            }
            catch (NoSuchCarException ex)
            {
                _logger.LogError($"Error in RemoveCarFromStore: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RemoveCarFromStore: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> ViewAllCarsInAllStores
        public async Task<List<Car>> ViewAllCarsInAllStores()
        {
            try
            {
                // Get all stores from the repository
                var allStores = await _rentalStoreRepository.GetAsync();

                // Flatten the list of cars from each store
                var allCars = allStores.SelectMany(store => store.CarStore?.Select(cs => cs.Car) ?? Enumerable.Empty<Car>())
                                       .ToList();

                return allCars;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewAllCarsInAllStores: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> ViewAllCarsInStore
        public async Task<List<CarStore>> ViewAllCarsInStore(int storeId)
        {
            try
            {
                // Get the store from the repository
                var store = await _rentalStoreRepository.GetAsyncById(storeId) ?? throw new NoSuchRentalStoreException();

                // Return the list of CarStore entries for the specified store
                return store.CarStore?.ToList() ?? new List<CarStore>();
            }
            catch (NoSuchRentalStoreException ex)
            {
                _logger.LogError($"Error in ViewAllCarsInStore: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewAllCarsInStore: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> ViewCarsInStore
        public async Task<List<Car>> ViewCarsInStore(int storeId)
        {
            try
            {
                // Get the store from the repository
                var store = await _rentalStoreRepository.GetAsyncById(storeId) ?? throw new NoSuchRentalStoreException();

                // Return the list of cars associated with the store
                return store.CarStore?.Select(cs => cs.Car).ToList() ?? new List<Car>();
            }
            catch (NoSuchRentalStoreException ex)
            {
                _logger.LogError($"Error in ViewCarsInStore: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewCarsInStore: {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
