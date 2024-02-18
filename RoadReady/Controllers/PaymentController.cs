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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentAdminService _paymentAdminService;
        private readonly IPaymentUserService _paymentUserService;

        public PaymentController(IPaymentAdminService paymentAdminService, IPaymentUserService paymentUserService)
        {
            _paymentAdminService = paymentAdminService;
            _paymentUserService = paymentUserService;
        }

        //Admin Action

        #region --> GetPaymentHistoryList
        [Authorize(Roles = "admin")]
        [HttpGet("admin/payment/history")]
        public async Task<ActionResult<List<Payment>>> GetPaymentHistoryList()
        {
            try
            {
                var paymentHistoryList = await _paymentAdminService.GetPaymentHistoryList();
                return Ok(paymentHistoryList);
            }
            catch (PaymentListEmptyException ex)
            {
                return NotFound("Payment history list is empty.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting the payment history list: {ex.Message}");
            }
        }
        #endregion

        #region --> GetPendingPayments
        [Authorize(Roles = "admin")]
        [HttpGet("admin/payment/pending")]
        public async Task<ActionResult<List<Payment>>> GetPendingPayments()
        {
            try
            {
                var pendingPayments = await _paymentAdminService.GetPendingPayments();
                return Ok(pendingPayments);
            }
            catch (NoSuchPaymentException ex)
            {
                return NotFound("No pending payments found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting pending payments: {ex.Message}");
            }
        }
        #endregion

        #region --> UpdatePaymentStatus
        [Authorize(Roles = "admin")]
        [HttpPut("admin/payment/update{paymentId}")]
        public async Task<ActionResult<Payment>> UpdatePaymentStatus(PaymentStatusDto paymentStatusDto)
        {
            try
            {
                var updatedPayment = await _paymentAdminService.UpdatePaymentStatus(paymentStatusDto.PaymentId, paymentStatusDto.PaymentStatus);
                return Ok(updatedPayment);
            }
            catch (NoSuchPaymentException ex)
            {
                return NotFound($"Payment with ID {paymentStatusDto.PaymentId} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the payment status: {ex.Message}");
            }
        }
        #endregion

        //User Action
        #region --> MakePayment
        [Authorize(Roles = "user")]
        [HttpPost("user/payment/make")]
        public async Task<ActionResult<Payment>> MakePayment(Payment payment)
        {
            try
            {
                var newPayment = await _paymentUserService.MakePayment(payment);

                //if (newPayment == null)
                //{
                //    return BadRequest("Failed to create payment.");
                //}
                //return newPayment;
                //201-Created or CreatedAtAction
                return CreatedAtAction(nameof(GetPaymentDetails), new { paymentId = newPayment.PaymentId }, newPayment);
            }
            catch (PaymentAlreadyExistsException ex)
            {
                return Conflict($"Payment with ID {payment.PaymentId} already exists.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while making the payment: {ex.Message}");
            }
        }
        #endregion

        #region --> GetPaymentDetails
        [Authorize(Roles = "user")]
        [HttpGet("user/payment/{id}")]
        public async Task<ActionResult<Payment>> GetPaymentDetails(int id)
        {
            try
            {
                var paymentDetails = await _paymentUserService.GetPaymentDetails(id);
                if (paymentDetails == null)
                {
                    return NotFound($"Payment with ID {id} not found.");
                }
                return Ok(paymentDetails);
            }
            catch (NoSuchPaymentException ex)
            {
                return NotFound($"Payment with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting the payment details: {ex.Message}");
            }
        }
        #endregion

        #region --> CancelPayment
        [Authorize(Roles = "user")]
        [HttpPut("user/payment/{id}/cancel")]
        public async Task<ActionResult<Payment>> CancelPayment(int id)
        {
            try
            {
                var cancelledPayment = await _paymentUserService.CancelPayment(id);
                if (cancelledPayment == null)
                {
                    return NotFound($"Payment with ID {id} not found.");
                }
                return Ok(cancelledPayment);
            }
            catch (NoSuchPaymentException ex)
            {
                return NotFound($"Payment with ID {id} not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while canceling the payment: {ex.Message}");
            }
        }
        #endregion

        #region --> GetUserPayments
        [Authorize(Roles = "user,admin")]
        [HttpGet("user/admin/userId/{userId}")]
        public async Task<ActionResult<List<Review>>> GetUserPayments(int userId)
        {
            try
            {
                var userPayments = await _paymentAdminService.GetUserPayments(userId);
                if (userPayments.Any())
                {
                    return Ok(userPayments);
                }
                else
                {
                    return NotFound($"No Payments found for user with ID {userId}.");
                }
            }
            catch (NoSuchReviewException ex)
            {
                return NotFound($"No Payments found for the user: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting user Payments: {ex.Message}");
            }
        }
        #endregion
    }
}
