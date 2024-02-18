using RoadReady.Models;
using RoadReady.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace RoadReady.Mappers
{
    public class RegisterToValidation
    {
        Validation validation;
        public RegisterToValidation(RegisterUserDto register)
        {
            validation = new Validation();
            validation.Username = register.Username;
            validation.Role = register.Role;
            GetPassword(register.Password);
        }
        public RegisterToValidation(RegisterAdminDto register)
        {
            validation = new Validation();
            validation.Username = register.Username;
            validation.Role = register.Role;
            GetPassword(register.Password);
        }
        void GetPassword(string password)
        {
            HMACSHA512 hmac = new HMACSHA512();
            validation.Key = hmac.Key;
            validation.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        public Models.Validation GetValidation()
        {
            return validation;
        }
    }
}
