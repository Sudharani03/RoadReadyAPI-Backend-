using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;

namespace RoadReady.Services
{
    public class ReviewService : IReviewAdminServices, IReviewUserServices
    {
        private readonly IRepository<int, Review> _reviewRepository;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IRepository<int, Review> reviewRepository, ILogger<ReviewService> logger)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
        }

        #region --> AddReview
        public async Task<Review> AddReview(Review review)
        {
            try
            {
                
                var addedReview = await _reviewRepository.Add(review);

                if (addedReview != null)
                {
                    return addedReview;
                }
                else
                {
                    _logger.LogInformation("Failed to add the review.");
                    throw new FailedReviewException();
                }
            }
            catch (FailedReviewException ex)
            {
                _logger.LogInformation($"Error adding review: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding a review: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> DeleteReview


        public async Task<Review> DeleteReview(int reviewid)
        {
            try
            {
                return await _reviewRepository.Delete(reviewid);
            }
            catch (NoSuchCarException ex)
            {
                _logger.LogWarning($"Review with ID {reviewid} not found.");
                throw; // Re-throw the exception
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting the car: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        #endregion

        #region --> GetAllReviews
        public async Task<List<Review>> GetAllReviews()
        {
            try
            {
                
                var allReviews = await _reviewRepository.GetAsync();

                if (allReviews.Any())
                {
                    return allReviews;
                }
                else
                {
                    _logger.LogInformation("No reviews found.");
                    throw new NoSuchReviewException();
                }
            }
            catch (NoSuchReviewException ex)
            {
                _logger.LogInformation($"No reviews found: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting all reviews: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region --> GetCarReviews
        public async Task<List<Review>> GetCarReviews(int carId)
        {
            try
            {
                
                var carReviews = (await _reviewRepository.GetAsync()).Where(r => r.CarId == carId).ToList();

                if (carReviews.Any())
                {
                    return carReviews;
                }
                else
                {
                    _logger.LogInformation($"No reviews found for car with ID {carId}.");
                    throw new NoSuchReviewException();
                }
            }
            catch (NoSuchReviewException ex)
            {
                _logger.LogInformation($"No reviews found for the car: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting car reviews: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> GetReviewDetails
        public async Task<Review> GetReviewDetails(int reviewId)
        {
            try
            {
                
                var review = await _reviewRepository.GetAsyncById(reviewId);

                if (review != null)
                {
                    return review;
                }
                else
                {
                    _logger.LogInformation($"Review with ID {reviewId} not found.");
                    throw new NoSuchReviewException();
                }
            }
            catch (NoSuchReviewException ex)
            {
                _logger.LogInformation($"No such review found: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting review details: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region --> GetUserReviews
        public async Task<List<Review>> GetUserReviews(int userId)
        {
            try
            {

                var userReviews = (await _reviewRepository.GetAsync()).Where(r => r.UserId == userId).ToList();

                if (userReviews.Any())
                {
                    return userReviews;
                }
                else
                {
                    _logger.LogInformation($"No reviews found for user with ID {userId}.");
                    throw new NoSuchReviewException();
                }
            }
            catch (NoSuchReviewException ex)
            {
                _logger.LogInformation($"No reviews found for the user: {ex.Message}");
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
