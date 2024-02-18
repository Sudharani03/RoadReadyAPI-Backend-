using RoadReady.Models;

namespace RoadReady.Interface
{
    public interface IReviewUserServices
    {
        public Task<Review> AddReview(Review review);
        public Task<Review> GetReviewDetails(int reviewId);
        public Task<List<Review>> GetUserReviews(int userId);
    }
}
