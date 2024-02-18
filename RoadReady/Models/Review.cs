using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoadReady.Models
{
    public class Review : IEquatable<Review>
    {
        public int ReviewId { get; set; }
        [Required(ErrorMessage = "Rating is Required")]
        public int Rating { get; set; }
        public string? Comments { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Today;

        //ForeignKey property for the one-to-many relationship
        public User? User { get; set; }
        public int? UserId { get; set; }
        public Car? Car { get; set; }
        public int? CarId { get; set; }

        //Default constructor
        public Review()
        {
            ReviewId = 0;
        }

        //Parameterized Constructor
        public Review(int reviewId, int rating, string comments, DateTime reviewDate, User user, int userId, Car car, int carId)
        {
            ReviewId = reviewId;
            Rating = rating;
            Comments = comments;
            ReviewDate = reviewDate;
            User = user;
            UserId = userId;
            Car = car;
            CarId = carId;
        }
        public Review( int rating, string comments, DateTime reviewDate, User user, int userId, Car car, int carId)
        {
            Rating = rating;
            Comments = comments;
            ReviewDate = reviewDate;
            User = user;
            UserId = userId;
            Car = car;
            CarId = carId;
        }

        public bool Equals(Review? other)
        {
            var review = other ?? new Review();
            return this.ReviewId.Equals(review.ReviewId);
        }

    }
}
