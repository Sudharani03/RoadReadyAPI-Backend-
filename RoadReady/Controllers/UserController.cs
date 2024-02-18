using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;
using RoadReady.Models.DTOs;
using RoadReady.Services;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAdminServices _userAdminService;
        private readonly IUserUserServices  _userUserService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserAdminServices userAdminService, IUserUserServices userUserService, ILogger<UserController> logger)
        {
            _userAdminService = userAdminService;
            _userUserService = userUserService;
            _logger = logger;
        }


        //Admin Action

        #region --> GetAllUsers
        [Authorize(Roles = "admin")]
        [HttpGet("admin/GetUser")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userAdminService.GetAllUsers();
                return Ok(users);
            }
            catch (UserListEmptyException)
            {
                return NotFound("User List is Empty.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting all users: {ex.Message}");
            }
        }
        #endregion

        //[HttpPost("admin/AddUser")]
        //public async Task<ActionResult<User>> AddUser(User user)
        //{
        //    try
        //    {
        //        var addedUser = await _userAdminService.AddUser(user);
        //        return Ok(addedUser);
        //    }
        //    catch (UserAlreadyExistsException)
        //    {
        //        return Conflict($"User with ID {user.UserId} already exists.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while adding user: {ex.Message}");
        //    }
        //}

        #region --> UpdateEmail
        [Authorize(Roles = "admin,user")]
        [HttpPut("{userId}/update-email")]
        public async Task<IActionResult> UpdateEmail(UserEmailDto userEmailDto)
        {
            try
            {
                var updatedEmail = await _userUserService.UpdateEmail(userEmailDto.UserId, userEmailDto.Email);
                if (updatedEmail != null)
                {
                    return Ok(updatedEmail);
                }
                else
                {
                    return NotFound($"User with ID {userEmailDto.UserId} not found.");
                }
            }
            catch (NoSuchUserException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the Users mail: {ex.Message}");
            }
        }
        #endregion

        #region --> UpdatePassword
        [Authorize(Roles = "admin,user")]
        [HttpPut("{userId}/update-password")]
        public async Task<ActionResult<User>> UpdatePassword(UserPasswordDto userPasswordDto)
        {
            try
            {
                var updatedUser = await _userUserService.UpdatePassword(userPasswordDto.UserId, userPasswordDto.Password);
                if (updatedUser != null)
                    return Ok(updatedUser);
                else
                     return NotFound($"User with ID {userPasswordDto.UserId} not found.");
            }
            catch (NoSuchUserException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the Users Password: {ex.Message}");
            }
        }
        #endregion

        #region --> UpdatePhoneNumber
        [Authorize(Roles = "admin,user")]
        [HttpPut("{userId}/update-phone-number")]
        public async Task<ActionResult<User>> UpdatePhoneNumber(UserPhoneNumberDto userPhoneNumberDto)
        {
            try
            {
                User updatedUser = await _userUserService.UpdatePhoneNumber(userPhoneNumberDto.UserId, userPhoneNumberDto.PhoneNumber);
                if (updatedUser != null)
                    return Ok(updatedUser);
                else
                    return NotFound($"User with ID {userPhoneNumberDto.UserId} not found.");
            }
            catch (NoSuchUserException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the Users PhoneNumber: {ex.Message}");
            }
        }
        #endregion

        //[HttpPut("{userId}/update-username")]
        //public async Task<ActionResult<User>> UpdateUserName(UserUserNameDto userUserNameDto)
        //{
        //    try
        //    {
        //        User updatedUser = await _userUserService.UpdateUserName(userUserNameDto.UserId, userUserNameDto.Username);
        //        if (updatedUser != null)
        //            return Ok(updatedUser);
        //        else
        //            return NotFound($"User with ID {userUserNameDto.UserId} not found.");
        //    }
        //    catch (NoSuchUserException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred while updating the UserName: {ex.Message}");
        //    }
        //}

        #region --> DeleteUser
        [Authorize(Roles = "admin,user")]
        [HttpDelete("admin/{userId}")]
        public async Task<ActionResult<User>> DeleteUser(int userId)
        {
            try
            {
                var deletedUser = await _userAdminService.DeleteUser(userId);
                return Ok(deletedUser);
            }
            catch (NoSuchUserException)
            {
                return NotFound($"User with ID {userId} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting user: {ex.Message}");
            }
        }
        #endregion

        //User Action

        #region --> GetUserById
        [Authorize(Roles = "user")]
        [HttpGet("user/GetUser/{userId}")]
        public async Task<ActionResult<User>> GetUserById(int userId)
        {
            try
            {
                var user = await _userUserService.GetUserById(userId);
                return Ok(user);
            }
            catch (NoSuchUserException)
            {
                return NotFound($"User with ID {userId} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting user by ID: {ex.Message}");
            }
        }
        #endregion
    }
}
