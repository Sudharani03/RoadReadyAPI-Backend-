using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace RoadReady.Models
{
    public class User : IEquatable<User>
    {

        public int UserId { get; set; }
        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "UserName is Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public byte[] Password { get; set; }
        [Required(ErrorMessage = "PhoneNumber is Required")]
        public string PhoneNumber { get; set; }
        public DateTime? RegistrationDate { get; set; } = DateTime.Today;

        [ForeignKey("Username")]
        public Validation? Validation { get; set; }

        // Navigation property for the one-to-many relationship
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<Payment>? Payments { get; set; }

        //Default constructor
        public User()
        {
            UserId = 0;
        }

        //Parameterized constructor 
        public User(int userId, string firstName, string lastName, string email, string username, string phoneNumber, DateTime? registrationDate)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
            PhoneNumber = phoneNumber;
            RegistrationDate = registrationDate;
        }


        public User(string firstName, string lastName, string email, string username, string phoneNumber, DateTime? registrationDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
            PhoneNumber = phoneNumber;
            RegistrationDate = registrationDate;
        }
        public bool Equals(User? other)
        {
            var user = other ?? new User();
            return this.UserId.Equals(user.UserId);
        }

    }
}
