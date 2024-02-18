using Microsoft.EntityFrameworkCore;
using RoadReady.Contexts;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class DiscountRepository : IRepository<int, Discount>
    {
        private readonly CarRentalDbContext _context;
        private readonly ILogger<DiscountRepository> _logger;
        public DiscountRepository(CarRentalDbContext context, ILogger<DiscountRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region ---> AddDiscount
        public async Task<Discount> Add(Discount item)
        {
            // Check if the DiscountID already exists
            var existingDiscount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == item.DiscountId);
            if (existingDiscount != null)
            {
                throw new DiscountAlreadyExistsException();
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

        #region ---> GetDiscountsList
        public async Task<List<Discount>> GetAsync()
        {
            var discounts = _context.Discounts.Include(c => c.Cars).ToList();
            //statement checks if the list is empty or not.
            if (!discounts.Any())
            {
                throw new DiscountListEmptyException();
            }
            return discounts;
        
        }
        #endregion

        #region ---> GetDiscountById

        public async Task<Discount> GetAsyncById(int key)
        {
            var discounts = await GetAsync();
            var discount = discounts.FirstOrDefault(d => d.DiscountId == key);
            if (discount != null)
                return discount;
            throw new NoSuchDiscountException();
        }

        #endregion

        #region ---> UpdateDiscount

        public async Task<Discount> Update(Discount item)
        {
            var Discount = await GetAsyncById(item.DiscountId);
            if (Discount != null)
            {
                _context.Entry<Discount>(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return item;
            }
            else
            {
                throw new NoSuchDiscountException();
            }
        }
        #endregion

        #region ---> DeleteDiscount
        public async Task<Discount> Delete(int key)
        {
            var discount = await GetAsyncById(key);
            if (discount != null)
            {
                _context?.Discounts.Remove(discount);
                await _context.SaveChangesAsync();
                return discount;
            }
            else
            {
                throw new NoSuchDiscountException();
            }
        }
        #endregion
    }
}

