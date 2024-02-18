using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models;
using RoadReady.Models.DTOs;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        //[HttpPost("Admin/Add")]
        //public async Task<ActionResult<Admin>> AddAdmin(Admin admin)
        //{
        //    try
        //    {
        //        admin = await _adminService.AddAdmin(admin);
        //        return new OkObjectResult(admin); 
        //    }
        //    catch (AdminAlreadyExistsException)
        //    {
        //        return Conflict($"Admin with ID {admin.AdminId} already exists.");
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the admin.");
        //    }
        //}

        #region --> deleteadmin

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/admins/{id}")]
        public async Task<ActionResult<Admin>> DeleteAdmin(int id)
        {
            try
            {
                return Ok(await _adminService.DeleteAdmin(id));
            }
            catch (NoSuchAdminException)
            {
                return NotFound($"Admin with ID {id} not found.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the Admin.");
            }
        }

        #endregion

        #region --> GetAdminById

        [Authorize(Roles = "admin")]
        [HttpGet("admin/admins/{id}")]
        public async Task<ActionResult<Admin>> GetAdminById(int id)
        {
            try
            {
                var admin = await _adminService.GetAdminById(id);
                if (admin == null)
                {
                    return NotFound($"Admin with ID {id} not found.");
                }
                return Ok(admin);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the admin details.");
            }
        }

        #endregion

        #region --> GetAllAdmins

        [Authorize(Roles = "admin")]
        [HttpGet("admin/GetAllAdmin")]
        public async Task<ActionResult<List<Admin>>> GetAllAdmins()
        {
            try
            {
                //OK--200(Success)
                return Ok(await _adminService.GetAllAdmins());
            }
            catch (AdminListEmptyException)
            {
                return NotFound("Admin List is Empty.");
            }
        }

        #endregion

        #region --> UpdateAdminEmail

        [Authorize(Roles = "admin")]
        [HttpPut("{adminId}/update-email")]
        public async Task<IActionResult> UpdateAdminEmail(AdminEmailDto adminEmailDto)
        {
            try
            {
                var updatedAdmin = await _adminService.UpdateAdminEmail(adminEmailDto.AdminId, adminEmailDto.Email);
                return Ok(updatedAdmin);
            }
            catch (NoSuchAdminException ex)
            {
                return NotFound($"Admin with ID {adminEmailDto.AdminId} not found.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        #endregion

        #region --> UpdatePassword

        [Authorize(Roles = "admin")]
        [HttpPut("{adminId}/update-password")]
        public async Task<IActionResult> UpdateAdminPassword(AdminPasswordDto adminPasswordDto)
        {
            try
            {
                var updatedAdmin = await _adminService.UpdateAdminPassword(adminPasswordDto.AdminId, adminPasswordDto.Password);
                return Ok(updatedAdmin);
            }
            catch (NoSuchAdminException ex)
            {
                return NotFound($"Admin with ID {adminPasswordDto.AdminId} not found.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        #endregion

        #region --> UpdatePhoneNumber

        [Authorize(Roles = "admin")]
        [HttpPut("{adminId}/update-phone-number")]
        public async Task<IActionResult> UpdateAdminPhoneNumber(AdminPhoneNumberDto adminPhoneNumberDto)
        {
            try
            {
                var updatedAdmin = await _adminService.UpdateAdminPhoneNumber(adminPhoneNumberDto.AdminId, adminPhoneNumberDto.PhoneNumber);
                return Ok(updatedAdmin);
            }
            catch (NoSuchAdminException ex)
            {
                return NotFound($"Admin with ID {adminPhoneNumberDto.AdminId} not found.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        #endregion


        //[HttpPut("{adminId}/update-username")]
        //public async Task<IActionResult> UpdateAdminUserName(AdminUsernameDto adminUserNameDto)
        //{
        //    try
        //    {
        //        var updatedAdmin = await _adminService.UpdateAdminUserName(adminUserNameDto.AdminId, adminUserNameDto.Username);
        //        return Ok(updatedAdmin);
        //    }
        //    catch (NoSuchAdminException ex)
        //    {
        //        return NotFound($"Admin with ID {adminUserNameDto.AdminId} not found.");
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred: {ex.Message}");
        //    }
        //}
    }

}

