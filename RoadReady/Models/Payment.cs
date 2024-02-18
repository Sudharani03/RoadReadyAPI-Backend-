using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RoadReady.Models
{
    public class Payment : IEquatable<Payment>
    {

        public int PaymentId { get; set; }
        [Required(ErrorMessage = "Payment Method is Required")]
        public string PaymentMethod { get; set; }
        [Required(ErrorMessage = "Payment Amount is Required")]
        public double PaymentAmount { get; set; }
        [Required(ErrorMessage = "Payment Status is Required")]
        public string PaymentStatus { get; set; }
        [Required(ErrorMessage = "TransactionId is Required")]
        public int TransactionId { get; set; }
        [Required(ErrorMessage = "TransactionDate is Required")]
        public DateTime TransactionDate { get; set; } = DateTime.Today;
        public string? CoupenCode { get; set; }

        //Links for Relations goes here
        //one to one relation --> payment and reservation
        [JsonIgnore]
        public Reservation? Reservation { get; set; }
        public int? ReservationId { get; set; }


        //Many to one relations 
        [JsonIgnore]
        public User? User { get; set; }
        public int? UserId { get; set; }

        //ForeignKey property for the one-to-many relationship
        [JsonIgnore]
        public Car? Car { get; set; }
        public int? CarId { get; set; }
        //Foreign key property for the one-to-many relationship
        [JsonIgnore]
        public Discount? Discount { get; set; }
        public int? DiscountId { get; set; }

        //Default Constructor 
        public Payment()
        {
            PaymentId = 0;
        }
        //Parameterized Constructor
        public Payment(int paymentId, string paymentMethod, double paymentAmount, string paymentStatus, int transactionId, DateTime transactionDate, string? coupenCode, int reservationId, int userId, int carId, int? discountId)
        {
            PaymentId = paymentId;
            PaymentMethod = paymentMethod;
            PaymentAmount = paymentAmount;
            PaymentStatus = paymentStatus;
            TransactionId = transactionId;
            TransactionDate = transactionDate;
            CoupenCode = coupenCode;
            ReservationId = reservationId;
            UserId = userId;
            CarId = carId;
            DiscountId = discountId;
        }
        public Payment( string paymentMethod, double paymentAmount, string paymentStatus, int transactionId, DateTime transactionDate, string? coupenCode, int reservationId, int userId, int carId, int? discountId)
        {
            PaymentMethod = paymentMethod;
            PaymentAmount = paymentAmount;
            PaymentStatus = paymentStatus;
            TransactionId = transactionId;
            TransactionDate = transactionDate;
            CoupenCode = coupenCode;
            ReservationId = reservationId;
            UserId = userId;
            CarId = carId;
            DiscountId = discountId;
        }
        public bool Equals(Payment other)
        {
            var payment = other ?? new Payment();
            return this.PaymentId.Equals(payment.PaymentId);
        }
    }
}
