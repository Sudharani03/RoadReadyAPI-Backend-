using System.ComponentModel.DataAnnotations;

namespace RoadReady.Models
{
    public class Car : IEquatable<Car>
    {
        public int CarId { get; set; }
        [Required(ErrorMessage = "Make is Required")]
        public string Make { get; set; }
        [Required(ErrorMessage = "Model is Required")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Year is Required")]
        public int Year { get; set; }

        public bool? Availability { get; set; } = true;
        [Required(ErrorMessage = "DailyRate is Required")]
        public double DailyRate { get; set; }
        public string? ImageURL { get; set; }
        public string Specification { get; set; }
        //navigation
        //one to many relation
        public Discount? Discount { get; set; }
        public int? DiscountId { get; set; }
        public ICollection<CarStore>? CarStore { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        //public object Discounts { get; set; }

        public List<Discount>? Discounts { get; set; } = new List<Discount>();

        //Default Constructor
        public Car()
        {
            CarId = 0;
        }

        //Parameterized Constructor
        public Car(int carId, string make, string model, int year, bool? availability, double dailRate, string? imageURL, string specification)
        {
            CarId = carId;
            Make = make;
            Model = model;
            Year = year;
            Availability = availability;
            DailyRate = dailRate;
            ImageURL = imageURL;
            Specification = specification;
        }
        public Car( string make, string model, int year, bool? availability, double dailRate, string? imageURL, string specification)
        {
            
            Make = make;
            Model = model;
            Year = year;
            Availability = availability;
            DailyRate = dailRate;
            ImageURL = imageURL;
            Specification = specification;
        }
        public bool Equals(Car? other)
        {
            var car = other ?? new Car();
            return this.CarId.Equals(car.CarId);
        }
    }
}
