using RoadReady.Models;

namespace RoadReady.Interface
{
    public interface IPaymentUserService
    {
        public Task<Payment> MakePayment(Payment payment);
        public Task<Payment> GetPaymentDetails(int id);
        public Task<Payment> CancelPayment(int id);
    }
}
