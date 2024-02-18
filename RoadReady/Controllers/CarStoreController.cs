using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarStoreController : ControllerBase
    {
        private readonly ICarStoreAdminServices _carStoreAdminService;
        private readonly ICarStoreUserServices _carStoreUserService;

        public CarStoreController(ICarStoreAdminServices carStoreAdminService, ICarStoreUserServices carStoreUserService)
        {
            _carStoreAdminService = carStoreAdminService;
            _carStoreUserService = carStoreUserService;
        }

        //Admin Action
        #region -->AddCarToStore
        [Authorize(Roles = "admin")]
        [HttpPost("admin/{storeId}/cars/{carId}")]
        public async Task<ActionResult<CarStore>> AddCarToStore(int storeId, int carId)
        {
            try
            {
                var carStore = await _carStoreAdminService.AddCarToStore(storeId, carId);
                return Ok(carStore);
            }
            catch (NoSuchCarException ex)
            {
                return NotFound($"Car with ID {carId} not found. {ex.Message}");
            }
            catch (NoSuchRentalStoreException ex)
            {
                return NotFound($"Rental store with ID {storeId} not found. {ex.Message}");
            }
            catch (CarAlreadyExistsException ex)
            {
                return Conflict($"Car with ID {carId} is already associated with store {storeId}. {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding car to store: {ex.Message}");
            }
        }
        #endregion

        #region --> RemoveCarFromStore
        [Authorize(Roles = "admin")]
        [HttpDelete("admin/{storeId}/cars/{carId}")]
        public async Task<ActionResult<CarStore>> RemoveCarFromStore(int storeId, int carId)
        {
            try
            {
                var removedCarStore = await _carStoreAdminService.RemoveCarFromStore(storeId, carId);
                if (removedCarStore == null)
                {
                    return NotFound($"Car with ID {carId} is not associated with store {storeId}");
                }
                return Ok(removedCarStore);
            }
            catch (NoSuchRentalStoreException ex)
            {
                return NotFound($"Rental store with ID {storeId} not found. {ex.Message}");
            }
            catch (NoSuchCarException ex)
            {
                return NotFound($"Car with ID {carId} not found. {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while removing car from store: {ex.Message}");
            }
        }
        #endregion

        #region --> ViewAllCarsInAllStores
        [Authorize(Roles = "admin")]
        [HttpGet("admin/CarsInallStores")]
        public async Task<ActionResult<List<Car>>> ViewAllCarsInAllStores()
        {
            try
            {
                var allCars = await _carStoreAdminService.ViewAllCarsInAllStores();
                return Ok(allCars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving all cars in all stores: {ex.Message}");
            }
        }
        #endregion

        #region --> ViewAllCarsInStore
        [Authorize(Roles = "admin")]
        [HttpGet("admin/{storeId}/cars")]
        public async Task<ActionResult<List<CarStore>>> ViewAllCarsInStore(int storeId)
        {
            try
            {
                var carsInStore = await _carStoreAdminService.ViewAllCarsInStore(storeId);
                return Ok(carsInStore);
            }
            catch (NoSuchRentalStoreException ex)
            {
                return NotFound($"Rental store with ID {storeId} not found. {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving cars in store: {ex.Message}");
            }
        }
        #endregion

        //User Action

        #region --> ViewCarsInStore
        [Authorize(Roles = "user")]
        [HttpGet("{storeId}/cars")]
        public async Task<ActionResult<List<Car>>> ViewCarsInStore(int storeId)
        {
            try
            {
                var carsInStore = await _carStoreUserService.ViewCarsInStore(storeId);
                return Ok(carsInStore);
            }
            catch (NoSuchRentalStoreException ex)
            {
                return NotFound($"Rental store with ID {storeId} not found. {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving cars in store: {ex.Message}");
            }
        }
        #endregion
    }
}
