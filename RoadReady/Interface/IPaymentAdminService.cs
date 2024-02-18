using RoadReady.Models;
using static RoadReady.Models.Payment;

namespace RoadReady.Interface
{
    public interface IPaymentAdminService
    {
        public Task<List<Payment>> GetPaymentHistoryList();
        public Task<List<Payment>> GetPendingPayments();
        public Task<List<Payment>> GetUserPayments(int userId);
        public Task<Payment> UpdatePaymentStatus(int paymentId, string paymentsStatus);
        
    }
}
