using RoadReady.Interface;
using RoadReady.Exceptions;
using RoadReady.Mappers;
using RoadReady.Models.DTOs;
using RoadReady.Models;
using System.Security.Cryptography;
using System.Text;

namespace RoadReady.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IRepository<int, Admin> _adminRepository;
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<string, Validation> _validationRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<ValidationService> _logger;


        public ValidationService(IRepository<int, Admin> adminRepository,
            IRepository<int, User> userRepository,
            IRepository<string, Validation> validationRepository,
            ITokenService tokenService,
            ILogger<ValidationService> logger)
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
            _validationRepository = validationRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<LoginValidationDto> Login(LoginValidationDto validation)
        {
            var myValidation = await _validationRepository.GetAsyncById(validation.Username);
            if (myValidation == null)
            {
                throw new InvalidValidationException();
            }
            var validationPassword = GetPasswordEncrypted(validation.Password, myValidation.Key);
            var checkPasswordMatch = ComparePasswords(myValidation.Password, validationPassword);
            if (checkPasswordMatch)
            {
                validation.Password = "";
                validation.Role = myValidation.Role;
                validation.Token = await _tokenService.GenerateToken(validation);
                return validation;
            }
            else
            {
                throw new InvalidValidationException();
            }
        }

        private bool ComparePasswords(byte[] password, byte[] validationPassword)
        {
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] != validationPassword[i])
                    return false;
            }
            return true;
        }

        private byte[] GetPasswordEncrypted(string password, byte[] key)
        {
            HMACSHA512 hmac = new HMACSHA512(key);
            var validationpassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return validationpassword;
        }

        public async Task<LoginValidationDto> RegisterUser(RegisterUserDto validation)
        {
            try
            {
                Validation myvalidation = new RegisterToValidation(validation).GetValidation();
                myvalidation = await _validationRepository.Add(myvalidation);
                User user = new RegisterToUser(validation).GetUser();
                user = await _userRepository.Add(user);

                LoginValidationDto result = new LoginValidationDto
                {
                    Username = myvalidation.Username,
                    Role = myvalidation.Role,
                };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user: {Username}", validation.Username);
                throw;
            }
        }

        public async Task<LoginValidationDto> RegisterAdmin(RegisterAdminDto validation)
        {
            try
            {
                Validation myvalidation = new RegisterToValidation(validation).GetValidation();
                myvalidation = await _validationRepository.Add(myvalidation);
                Admin admin = new RegisterToAdmin(validation).GetAdmin();
                admin = await _adminRepository.Add(admin);
                LoginValidationDto result = new LoginValidationDto
                {
                    Username = myvalidation.Username,
                    Role = myvalidation.Role,
                };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user: {Username}", validation.Username);
                throw;
            }
        }
    }
}
