using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;
using RoadReady.Repositories;

namespace RoadReady.Services
{
    public class PaymentService : IPaymentAdminService, IPaymentUserService
    {
        private readonly IRepository<int, Payment> _paymentRepository;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IRepository<int,Payment> paymentRepository, ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        #region ---> MakePayment
        public async Task<Payment> MakePayment(Payment payment)
        {
            try
            {
                return await _paymentRepository.Add(payment);
            }
            catch (PaymentAlreadyExistsException ex)
            {
                _logger.LogWarning($"Payment with ID {payment.PaymentId} already exists.");
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding the payment: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        #endregion

        #region ---> CancelPayment
        public async Task<Payment> CancelPayment(int id)
        {
            try
            {

                var payment = await _paymentRepository.GetAsyncById(id);

                if (payment != null)
                {
                    // Update payment status or perform other cancellation logic setting the status to "Cancelled"
                    payment.PaymentStatus = "Cancelled";

                    // Save the updated payment in the repository
                    return await _paymentRepository.Update(payment);
                }
                else
                {
                    _logger.LogInformation($"Payment with ID {id} not found.");
                    throw new NoSuchPaymentException();
                }
            }
            catch (NoSuchPaymentException ex)
            {
                _logger.LogInformation($"No such payment found: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while canceling the payment: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region ---> GetPaymentDetails
        public async Task<Payment> GetPaymentDetails(int id)
        {
            try
            {
                return await _paymentRepository.GetAsyncById(id);
            }
            catch (NoSuchPaymentException ex)
            {
                _logger.LogWarning($"Payment with ID {id} not found.");
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting the payment by ID: {ex.Message}");
                throw; // Re-throw the exception
            }
        }
        #endregion

        #region ---> GetPaymentHistoryList

        public async Task<List<Payment>> GetPaymentHistoryList()
        {
            try
            {
                return await _paymentRepository.GetAsync();
            }
            catch (PaymentListEmptyException ex)
            {
                _logger.LogInformation("Payment history list is empty.");
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting the payment history list: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        #endregion

        #region ---> UpdatePaymentStatus
        public async Task<Payment> UpdatePaymentStatus(int paymentId, string  paymentsStatus)
        {
            try
            {
                
                // Get the payment by ID, update the status, and save changes
                var payment = await _paymentRepository.GetAsyncById(paymentId);
                payment.PaymentStatus = paymentsStatus;
                return await _paymentRepository.Update(payment);
            }
            catch (NoSuchPaymentException ex)
            {
                _logger.LogWarning($"Payment with ID {paymentId} not found.");
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating the payment status: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        #endregion

        #region ---> GetPendingPayments

        public async Task<List<Payment>> GetPendingPayments()
        {
            try
            {
                
                var pendingPayments = await _paymentRepository.GetAsync(); 

                // Filter payments based on pending status
                var filteredPayments = pendingPayments.Where(payment => payment.PaymentStatus.Equals("Pending")).ToList();

                if (filteredPayments.Any())
                {
                    return filteredPayments;
                }
                else
                {
                    _logger.LogInformation("No pending payments found.");
                    throw new NoSuchPaymentException();
                }
            }
            catch (NoSuchPaymentException ex)
            {
                _logger.LogInformation($"No pending payments found: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting pending payments: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> GetUserPayments 
        public async Task<List<Payment>> GetUserPayments(int userId)
        {
            try
            {

                var userPayments = (await _paymentRepository.GetAsync()).Where(r => r.UserId == userId).ToList();

                if (userPayments.Any())
                {
                    return userPayments;
                }
                else
                {
                    _logger.LogInformation($"No Payments found for user with ID {userId}.");
                    throw new NoSuchReviewException();
                }
            }
            catch (NoSuchReviewException ex)
            {
                _logger.LogInformation($"No Payments found for the user: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting user reviews: {ex.Message}");
                throw;
            }
        }
        #endregion


    }

}
