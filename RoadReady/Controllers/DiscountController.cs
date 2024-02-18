using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;
using RoadReady.Models.DTOs;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountAdminServices _discountService;

        public DiscountController(IDiscountAdminServices discountService)
        {
            _discountService = discountService;
        }

        //Admin Action

        #region --> AddNewDiscount
        [Authorize(Roles = "admin")]
        [HttpPost("admin/discounts/add")]
        public async Task<ActionResult<Discount>> AddNewDiscount(Discount discount)
        {
            try
            {
                var addedDiscount = await _discountService.AddNewDiscount(discount);
                return Ok(addedDiscount);
            }
            catch (DiscountAlreadyExistsException)
            {
                return Conflict($"Discount with ID {discount.DiscountId} already exists.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding a new discount: {ex.Message}");
            }
        }
        #endregion

        #region --> UpdateDiscountEndDate
        [Authorize(Roles = "admin")]
        [HttpPut("{discountId}/update-end-date")]
        public async Task<IActionResult> UpdateDiscountEndDate(DiscountEndDateDto discountEndDateDto)
        {
            try
            {
                var updatedDiscount = await _discountService.UpdateDiscountEndDate(discountEndDateDto.DiscountId, discountEndDateDto.EndDateOfDiscount);
                return Ok(updatedDiscount);
            }
            catch (NoSuchDiscountException ex)
            {
                return NotFound($"Discount with ID {discountEndDateDto.DiscountId} not found.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        #endregion

        #region --> UpdateDiscountPercentage
        [Authorize(Roles = "admin")]
        [HttpPut("{discountId}/update-percentage")]
        public async Task<IActionResult> UpdateDiscountPercentage(DiscountPercentageDto discountPercentageDto)
        {
            try
            {
                var updatedDiscount = await _discountService.UpdateDiscountPercentage(discountPercentageDto.DiscountId, discountPercentageDto.DiscountPercentage);
                return Ok(updatedDiscount);
            }
            catch (NoSuchDiscountException ex)
            {
                return NotFound($"Discount with ID {discountPercentageDto.DiscountId} not found.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        #endregion

        #region --> DeactivateDiscount
        [Authorize(Roles = "admin")]
        [HttpPut("admin/discounts/{discountId}/deactivate")]
        public async Task<ActionResult<bool>> DeactivateDiscount(int discountId)
        {
            try
            {
                var isDeactivated = await _discountService.DeactivateDiscount(discountId);
                if (isDeactivated)
                {
                    return Ok($"Discount with ID {discountId} has been successfully deactivated.");
                }
                else
                {
                    return NotFound($"Discount with ID {discountId} not found.");
                }
            }
            catch (NoSuchDiscountException ex)
            {
                return NotFound($"Discount with ID {discountId} not found. {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deactivating discount: {ex.Message}");
            }
        }
        #endregion

        #region --> ViewAllDiscounts
        [Authorize(Roles = "admin,user")]
        [HttpGet("admin/discounts/alldiscounts")]
        public async Task<ActionResult<List<Discount>>> ViewAllDiscounts()
        {
            try
            {
                var allDiscounts = await _discountService.ViewAllDiscounts();
                return Ok(allDiscounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving all discounts: {ex.Message}");
            }
        }
        #endregion

        #region --> AssignDiscountToCar
        [Authorize(Roles = "admin")]
        [HttpPost("admin/discount/assignDiscountCar")]
        public async Task<ActionResult<bool>> AssignDiscountToCar(int discountId, int carId)
        {
            try
            {
                var isAssigned = await _discountService.AssignDiscountToCar(discountId, carId);
                if (isAssigned)
                {
                    return Ok($"Discount with ID {discountId} has been successfully assigned to car with ID {carId}.");
                }
                else
                {
                    return NotFound($"Discount with ID {discountId} or car with ID {carId} not found.");
                }
            }
            catch (NoSuchDiscountException ex)
            {
                return NotFound($"Discount with ID {discountId} not found. {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while assigning discount to car: {ex.Message}");
            }
        }
        #endregion

        #region --> ViewCarsWithDiscounts
        [Authorize(Roles = "admin")]
        [HttpGet("admin/discount/carwithdiscounts")]
        public async Task<ActionResult<List<Car>>> ViewCarsWithDiscounts()
        {
            try
            {
                var carsWithDiscounts = await _discountService.ViewCarsWithDiscounts();
                if (carsWithDiscounts == null)
                {
                    throw new DiscountListEmptyException();
                }
                return Ok(carsWithDiscounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving cars with discounts: {ex.Message}");
            }
        }
        #endregion


        //User Action

        #region --> ViewAvailableDiscounts
        [Authorize(Roles = "user")]
        [HttpGet("user/Discount/available")]
        public async Task<ActionResult<List<Discount>>> ViewAvailableDiscounts()
        {
            try
            {
                var availableDiscounts = await _discountService.ViewAvailableDiscounts();
                return Ok(availableDiscounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving available discounts: {ex.Message}");
            }
        }
        #endregion

        //[HttpPost("user/discount/{reservationId}/applydiscount")]
        //public async Task<ActionResult<Reservation>> ApplyDiscountToReservation(int reservationId, string discountCode)
        //{
        //    try
        //    {
        //        var updatedReservation = await _discountService.ApplyDiscountToReservation(reservationId, discountCode);
        //        return Ok(updatedReservation);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while applying discount to reservation: {ex.Message}");
        //    }
        //}

        #region --> ViewDiscountDetails
        [Authorize(Roles = "user")]
        [HttpGet("user/discount/{discountId}")]
        public async Task<ActionResult<Discount>> ViewDiscountDetails(int discountId)
        {
            try
            {
                var discountDetails = await _discountService.ViewDiscountDetails(discountId);
                if (discountDetails == null)
                {
                    throw new DiscountListEmptyException();
                }
                return Ok(discountDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving discount details: {ex.Message}");
            }
        }
        #endregion
    }
}
