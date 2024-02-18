using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoadReady.Models
{
    public class Discount : IEquatable<Discount>
    {
        public int DiscountId { get; set; }
        public string? DiscountName { get; set; }
        [Required(ErrorMessage = "CarId is Required")]
        public int CarId { get; set; }
        //public Car? Car { get; set; }
        [Required(ErrorMessage = "StartDate of Discount is Required")]
        public DateTime StartDateOfDiscount { get; set; }
        [Required(ErrorMessage = "EndDate of Discount is Required")]
        public DateTime EndDateOfDiscount { get; set; }
        public string? CoupenCode { get; set; }
        [Required(ErrorMessage = "Discount Percentage is Required")]
        public double DiscountPercentage { get; set; }
        public int? ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        //Navigation property for the one-to-many relationship
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<Car>? Cars { get; set; }

        //Default Constructor 
        public Discount()
        {
            DiscountId = 0;
        }

        //Parameterized Constructor 
        public Discount(int discountId, string? discountName, int carId, DateTime startDateOfDiscount, DateTime endDateOfDiscount, string? coupenCode, double discountPercentage)
        {
            DiscountId = discountId;
            DiscountName = discountName;
            CarId = carId;
            StartDateOfDiscount = startDateOfDiscount;
            EndDateOfDiscount = endDateOfDiscount;
            CoupenCode = coupenCode;
            DiscountPercentage = discountPercentage;
        }
        public Discount( string? discountName, int carId, DateTime startDateOfDiscount, DateTime endDateOfDiscount, string? coupenCode, double discountPercentage)
        {
            DiscountName = discountName;
            CarId = carId;
            StartDateOfDiscount = startDateOfDiscount;
            EndDateOfDiscount = endDateOfDiscount;
            CoupenCode = coupenCode;
            DiscountPercentage = discountPercentage;
        }
        public bool Equals(Discount? other)
        {
            var discount = other ?? new Discount();
            return this.DiscountId.Equals(discount.DiscountId);
        }
    }
}
