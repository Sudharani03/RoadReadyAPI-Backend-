using RoadReady.Models;
using RoadReady.Models.DTOs;

namespace RoadReady.Interface
{
    public interface IUserAdminServices
    {
        Task<List<User>> GetAllUsers();
        Task<User> AddUser(User user);
        
        Task<User> DeleteUser(int userId);

        //Task<LoginUserDto> Login(LoginUserDto user);
        //Task<LoginUserDto> RegisterAdmin(RegisterAdminDto user);
        //Task<LoginUserDto> RegisterUser(RegisterUserDto user);
    }
}
