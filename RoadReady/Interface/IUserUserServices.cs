using RoadReady.Models;
using RoadReady.Models.DTOs;

namespace RoadReady.Interface
{
    public interface IUserUserServices
    {
        Task<User> GetUserById(int userId);
        Task<List<Review>> GetUserReviews(int userId);
        Task<List<Reservation>> GetUserReservations(int userId);
        Task<List<Payment>> GetUserPayments(int userId);
        Task<User> UpdateUserName(int userId, string userName);
        Task<User> UpdatePassword(int userId, byte[] password);
        Task<User> UpdateEmail(int userId, string email);
        Task<User> UpdatePhoneNumber(int userId, string phoneNumber);

        //Task<LoginUserDto> Login(LoginUserDto user);
        //Task<LoginUserDto> RegisterAdmin(RegisterAdminDto user);
        //Task<LoginUserDto> RegisterUser(RegisterUserDto user);

    }
}
