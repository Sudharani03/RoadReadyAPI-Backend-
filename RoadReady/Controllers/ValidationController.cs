using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadReady.Exceptions;
using RoadReady.Interface;
using RoadReady.Models.DTOs;

namespace RoadReady.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly ILogger<ValidationController> _logger;

        public ValidationController(IValidationService validationService, ILogger<ValidationController> logger)
        {
            _validationService = validationService;
            _logger = logger;
        }

        [Route("/Register_User")]
        [HttpPost]
        public async Task<LoginValidationDto> RegisterUser(RegisterUserDto validation)
        {
            var result = await _validationService.RegisterUser(validation);
            return result;
        }

        [Route("/Register_Admin")]
        [HttpPost]
        public async Task<LoginValidationDto> RegisterAdmin(RegisterAdminDto validation)
        {
            var result = await _validationService.RegisterAdmin(validation);
            return result;
        }

        [Route("/Login")]
        [HttpPost]
        public async Task<ActionResult<LoginValidationDto>> Login(LoginValidationDto validation)
        {
            try
            {
                var result = await _validationService.Login(validation);
                return Ok(result);
            }
            catch (InvalidValidationException iuse)
            {
                _logger.LogCritical(iuse.Message);
                return Unauthorized("Invalid username or password");
            }

        }
    }
}
