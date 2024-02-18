using RoadReady.Models;
using RoadReady.Models.DTOs;
using System.Text;

namespace RoadReady.Mappers
{
    public class RegisterToAdmin
    {
        Admin admin;
        public RegisterToAdmin(RegisterAdminDto register)
        {
            admin = new Admin();
            admin.FirstName = register.FirstName;
            admin.LastName = register.LastName;
            admin.Email = register.Email;
            admin.Username = register.Username;
            admin.PhoneNumber = register.PhoneNumber;
            admin.Password = Encoding.UTF8.GetBytes(register.Password);
        }
        public Admin GetAdmin()
        {
            return admin;
        }
    }
}
