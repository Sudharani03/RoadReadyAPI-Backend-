using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Mappers;
using RoadReady.Models;
using RoadReady.Models.DTOs;
using RoadReady.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace RoadReady.Services
{
    public class UserService : IUserUserServices, IUserAdminServices
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly ILogger<UserService> _logger;


        public UserService(IRepository<int, User> userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;

            
        }

        #region ---> AddUser
        public async Task<User> AddUser(User user)
        {
            try
            {
                return await _userRepository.Add(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddUser: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region ---> DeleteUser

        public async Task<User> DeleteUser(int userId)
        {
            try
            {
                return await _userRepository.Delete(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteUser: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region --> GetAllUsers
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return await _userRepository.GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllUsers: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> GetUserById
        public async Task<User> GetUserById(int userId)
        {
            try
            {
                return await _userRepository.GetAsyncById(userId) ?? throw new NoSuchUserException();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUserById: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> GetUserPayments
        public async Task<List<Payment>> GetUserPayments(int userId)
        {
            try
            {
                var user = await _userRepository.GetAsyncById(userId) ?? throw new NoSuchUserException();
                return user.Payments?.ToList() ?? new List<Payment>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUserPayments: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> GetUserReservations
        public async Task<List<Reservation>> GetUserReservations(int userId)
        {
            try
            {
                var user = await _userRepository.GetAsyncById(userId) ?? throw new NoSuchUserException();
                return user.Reservations?.ToList() ?? new List<Reservation>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUserReservations: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> GetUserReviews
        public async Task<List<Review>> GetUserReviews(int userId)
        {
            try
            {
                var user = await _userRepository.GetAsyncById(userId) ?? throw new NoSuchUserException();
                return user.Reviews?.ToList() ?? new List<Review>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUserReviews: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> UpdateEmail
        public async Task<User> UpdateEmail(int userId, string email)
        {
            try
            {
                User user = await _userRepository.GetAsyncById(userId);

                if (user == null)
                {
                    _logger.LogWarning($"User with ID {userId} not found.");
                    throw new NoSuchUserException();
                }

                user.Email = email;

                await _userRepository.Update(user);

                return user;
            }
            catch (NoSuchUserException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating email for user with ID {userId}: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> UpdatePassword
        public async Task<User> UpdatePassword(int userId, byte[] password)
        {
            try
            {
                User user = await _userRepository.GetAsyncById(userId);

                if (user == null)
                {
                    throw new NoSuchUserException();
                }

                user.Password = password;

                await _userRepository.Update(user);

                return user;
            }
            catch (NoSuchUserException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating password for user with ID {userId}: {ex.Message}");
                throw;
            }
        }

        
        #endregion

        #region --> UpdatePhoneNumber
        public async Task<User> UpdatePhoneNumber(int userId, string phoneNumber)
        {
            try
            {
                User user = await _userRepository.GetAsyncById(userId);

                if (user == null)
                {
                    throw new NoSuchUserException();
                }

                user.PhoneNumber = phoneNumber;

                await _userRepository.Update(user);

                return user;
            }
            catch (NoSuchUserException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating phone number for user with ID {userId}: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region --> UpdateUserName
        public async Task<User> UpdateUserName(int userId, string userName)
        {
            try
            {
                User user = await _userRepository.GetAsyncById(userId);

                if (user == null)
                {
                    throw new NoSuchUserException();
                }

                user.Username = userName;

                await _userRepository.Update(user);

                return user;
            }
            catch (NoSuchUserException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating username for user with ID {userId}: {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
