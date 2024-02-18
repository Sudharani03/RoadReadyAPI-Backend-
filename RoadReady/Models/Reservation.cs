using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoadReady.Models
{
    public class Reservation : IEquatable<Reservation>
    {

        public int ReservationId { get; set; }
        [Required(ErrorMessage = "PickUpDateTime is Required")]
        public DateTime PickUpDateTime { get; set; }
        [Required(ErrorMessage = "DropOffDateTime is Required")]
        public DateTime DropOffDateTime { get; set; }
        [Required(ErrorMessage = "Status is Required")]
        public string Status { get; set; }
        [Required(ErrorMessage = "DropOffStoreLocation is Required")]
        public string PickUpLocation { get; set; }
        [Required(ErrorMessage = "DropOffStoreLocation is Required")]
        public string DropOffLocation { get; set; }
        [Required(ErrorMessage = "TotalPrice is Required")]
        public double TotalPrice { get; set; }

        //one to one relation --> payment and reservation 
        public Payment? Payment { get; set; }
        public int? PaymentId { get; set; }
        //many to one 

        public User? User { get; set; }
        public int? UserId { get; set; }
        public Car? Car { get; set; }
        public int? CarId { get; set; }
        public ICollection<Discount>? AppliedDiscounts { get; set; }

        //Default Constructor 
        public Reservation()
        {
            ReservationId = 0;
        }

        //Parameterized Constructor 
        public Reservation(int reservationId, int? carId, Car? car, int? userId, User? user,string status, DateTime pickUpDateTime, DateTime dropOffDateTime, double totalPrice)
        {
            ReservationId = reservationId;
            CarId = carId;
            Car = car;
            UserId = userId;
            User = user;
            Status = status;
            PickUpDateTime = pickUpDateTime;
            DropOffDateTime = dropOffDateTime;
            TotalPrice = totalPrice;
        }
        public Reservation( int carId, Car? car, int userId, User? user,string status, DateTime pickUpDateTime, DateTime dropOffDateTime, double totalPrice)
        {
            CarId = carId;
            Car = car;
            UserId = userId;
            User = user;
            Status = status;
            PickUpDateTime = pickUpDateTime;
            DropOffDateTime = dropOffDateTime;
            TotalPrice = totalPrice;
        }
        public bool Equals(Reservation? other)
        {
            var reservation = other ?? new Reservation();
            return this.ReservationId.Equals(reservation.ReservationId);
        }
    }
}
