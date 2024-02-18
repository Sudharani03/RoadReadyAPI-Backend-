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
    public class RentalStoreController : ControllerBase
    {
        private readonly IRentalStoreAdminService _rentalStoreAdminService;
        private readonly IRentalStoreUserService _rentalStoreUserService;

        public RentalStoreController(IRentalStoreAdminService rentalStoreAdminService, IRentalStoreUserService rentalStoreUserService)
        {
            _rentalStoreAdminService = rentalStoreAdminService;
            _rentalStoreUserService = rentalStoreUserService;
        }

        #region --> GetAllRentalStores
        [Authorize(Roles = "admin,user")]
        [HttpGet("Admin/GetStoresList")]
        public async Task<ActionResult<List<RentalStore>>> GetAllRentalStores()
        {
            try
            {
                var rentalStores = await _rentalStoreAdminService.GetAllRentalStores();
                if (rentalStores == null || rentalStores.Count == 0)
                {
                    return NotFound("No rental stores found.");
                }
                return Ok(rentalStores);
            }
            catch (RentalStoreListEmptyException)
            {
                return NotFound("Rental store list is empty.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting all rental stores: {ex.Message}");
            }
        }
        #endregion

        #region --> GetCarsInStore
        [Authorize(Roles = "admin,user")]
        [HttpGet("admin/cars/{storeId}")]
        public async Task<ActionResult<List<CarStore>>> GetCarsInStore(int storeId)
        {
            try
            {
                var carStores = await _rentalStoreAdminService.GetCarsInStore(storeId);
                if (carStores == null || carStores.Count == 0)
                {
                    return NotFound($"No cars found in rental store with ID {storeId}.");
                }
                return Ok(carStores);
            }
            catch (NoSuchCarException)
            {
                return NotFound($"No cars found in rental store with ID {storeId}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting cars in the rental store: {ex.Message}");
            }
        }
        #endregion

        #region --> AddRentalStore
        [Authorize(Roles = "user")]
        [HttpPost("user/addRental")]
        public async Task<ActionResult<RentalStore>> AddRentalStore(RentalStore rentalStore)
        {
            try
            {
                var addedRentalStore = await _rentalStoreAdminService.AddRentalStore(rentalStore);
                return CreatedAtAction(nameof(GetRentalStoreById), new { id = addedRentalStore.StoreId }, addedRentalStore);
            }
            catch (RentalStoreAlreadyExistsException ex)
            {
                return Conflict($"Rental store with ID {rentalStore.StoreId} already exists.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the rental store: {ex.Message}");
            }
        }
        #endregion

        #region --> UpdateRentalStoreDetails
        [Authorize(Roles = "user")]
        [HttpPut("user/update")]
        public async Task<ActionResult<RentalStore>> UpdateRentalStoreDetails(RentalStore rentalStore)
        {
            try
            {
                var updatedRentalStore = await _rentalStoreAdminService.UpdateRentalStoreDetails(rentalStore.StoreId, rentalStore.PickUpLocation, rentalStore.DropOffLocation);
                if (updatedRentalStore == null)
                {
                    return NotFound($"Rental store with ID {rentalStore.StoreId} not found.");
                }
                return Ok(updatedRentalStore);
            }
            catch (NoSuchRentalStoreException ex)
            {
                return NotFound($"Rental store with ID {rentalStore.StoreId} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the rental store details: {ex.Message}");
            }
        }
        #endregion

        #region --> RemoveRentalStore
        [Authorize(Roles = "user")]
        [HttpDelete("user/{id}")]
        public async Task<ActionResult<RentalStore>> RemoveRentalStore(int id)
        {
            try
            {
                var removedRentalStore = await _rentalStoreAdminService.RemoveRentalStore(id);
                if (removedRentalStore == null)
                {
                    return NotFound($"Rental store with ID {id} not found.");
                }
                return Ok(removedRentalStore);
            }
            catch (NoSuchRentalStoreException ex)
            {
                return NotFound($"Rental store with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the rental store: {ex.Message}");
            }
        }
        #endregion

        #region --> GetRentalStoreById
        [Authorize(Roles = "user")]
        [HttpGet("user/{storeId}")]
        public async Task<ActionResult<RentalStore>> GetRentalStoreById(int storeId)
        {
            try
            {
                var rentalStore = await _rentalStoreAdminService.GetRentalStoreById(storeId);
                if (rentalStore == null)
                {
                    return NotFound($"Rental store with ID {storeId} not found.");
                }
                return Ok(rentalStore);
            }
            catch (NoSuchRentalStoreException ex)
            {
                return NotFound($"No rental store found with ID {storeId}: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting rental store by ID: {ex.Message}");
            }
        }
        #endregion
    }
}
