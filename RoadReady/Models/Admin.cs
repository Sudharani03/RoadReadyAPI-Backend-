using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoadReady.Models
{
    public class Admin : IEquatable<Admin>
    {

        public int AdminId { get; set; }
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

        [ForeignKey("Username")]
        public Validation? Validation { get; set; }
        public User? user { get; set; }

        //Default constructor
        public Admin()
        {
            AdminId = 0;
        }

        //Parameterized Constructor
        public Admin(int adminId, string firstName, string lastName, string email, string username, string phoneNumber)
        {
            AdminId = adminId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
            PhoneNumber = phoneNumber;
        }
        public Admin(string firstName, string lastName, string email, string username, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
            PhoneNumber = phoneNumber;
        }
        public bool Equals(Admin? other)
        {
            var admin = other ?? new Admin();
            return this.AdminId.Equals(admin.AdminId);
        }
    }
}
