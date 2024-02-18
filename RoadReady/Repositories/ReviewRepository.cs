using Microsoft.EntityFrameworkCore;
using RoadReady.Contexts;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class ReviewRepository : IRepository<int, Review>
    {
        private readonly CarRentalDbContext _context;
        private readonly ILogger<ReviewRepository> _logger;
        public ReviewRepository(CarRentalDbContext context, ILogger<ReviewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region --> AddReviews
        public async Task<Review> Add(Review item)
        {
            // Check if the ReviewID already exists
            var existingReview = await _context.Reviews.FirstOrDefaultAsync(rev => rev.ReviewId == item.ReviewId);
            if (existingReview != null)
            {
                throw new ReviewAlreadyExistsException();
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

        #region --> GetReviewsList
        public async Task<List<Review>> GetAsync()
        {
            var reviews = _context.Reviews.Include(u => u.User).Include(c => c.Car).ToList();
            //statement checks if the list is empty or not.
            if (!reviews.Any())
            {
                throw new ReviewListEmptyException();
            }
            return reviews;
        }
        #endregion

        #region --> GetReviewById
        public async Task<Review> GetAsyncById(int key)
        {
            var reviews = await GetAsync();
            var review = reviews.FirstOrDefault(rev => rev.ReviewId == key);
            if (review != null)
                return review;
            throw new NoSuchReviewException();
        }
        #endregion

        #region --> UpdateReview
        public async Task<Review> Update(Review item)
        {
            var Review = await GetAsyncById(item.ReviewId);
            if (Review != null)
            {
                _context.Entry<Review>(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return item;
            }
            else
            {
                throw new NoSuchReviewException();
            }
        }
        #endregion

        #region --> DeleteReview
        public async Task<Review> Delete(int key)
        {
            var review = await GetAsyncById(key);
            if (review != null)
            {
                _context?.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return review;
            }
            else
            {
                throw new NoSuchReviewException();
            }
        }
        #endregion
    }
}
