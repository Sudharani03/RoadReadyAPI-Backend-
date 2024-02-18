using RoadReady.Models.DTOs;
using RoadReady.Models;
using System.Text;

namespace RoadReady.Mappers
{
    public class RegisterToUser
    {
        User user;
        public RegisterToUser(RegisterUserDto register)
        {
            user = new User();
            user.FirstName = register.FirstName;
            user.LastName = register.LastName;
            user.Email = register.Email;
            user.Username = register.Username;
            user.PhoneNumber = register.PhoneNumber;
            user.Password = Encoding.UTF8.GetBytes(register.Password);
        }
        public User GetUser()
        {
            return user;
        }
    }
}
