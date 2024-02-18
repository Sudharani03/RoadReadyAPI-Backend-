using Microsoft.EntityFrameworkCore;
using RoadReady.Contexts;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class PaymentRepository : IRepository<int, Payment>
    {
        private readonly CarRentalDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;
        public PaymentRepository(CarRentalDbContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region ---> AddPayment
        public async Task<Payment> Add(Payment item)
        {
            // Check if the DiscountID already exists
            var existingPayment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == item.PaymentId);
            if (existingPayment != null)
            {
                throw new PaymentAlreadyExistsException();
            }
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (DbUpdateException ex)
            {
                // Log the exception and return null
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                // Log the exception and return null
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
        #endregion


        #region ---> GetPaymentList
        public async Task<List<Payment>> GetAsync()
        {
            var payments = _context.Payments.Include(r => r.Reservation).Include(u => u.User).Include(c => c.Car).Include(d => d.Discount).ToList();
            //statement checks if the list is empty or not.
            if (!payments.Any())
            {
                _logger.LogInformation("Payment list is empty.");
                throw new PaymentListEmptyException();
            }
            return payments;
        }

        #endregion


        #region ---> GetPaymentById
        public async Task<Payment> GetAsyncById(int key)
        {
            var payments = await GetAsync();
            var payment = payments.FirstOrDefault(p => p.PaymentId == key);
            if (payment != null)
                return payment;
            throw new NoSuchPaymentException();
        }

        #endregion


        #region ---> UpdatePayment 
        public async Task<Payment> Update(Payment item)
        {
            var Payment = await GetAsyncById(item.PaymentId);
            if (Payment != null)
            {
                _context.Entry<Payment>(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return item;
            }
            else
            {
                throw new NoSuchPaymentException();
            }
        }

        #endregion


        #region ---> DeletePayment 
        public async Task<Payment> Delete(int key)
        {
            var payment = await GetAsyncById(key);
            if (payment != null)
            {
                _context?.Payments.Remove(payment);
                await _context.SaveChangesAsync();
                return payment;
            }
            else
            {
                throw new NoSuchPaymentException();
            }
        }
        #endregion
    }
}

