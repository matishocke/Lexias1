using Lexias.Services.AuthAPI.Models.Dto;
using Lexias.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lexias.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
        }






        //Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            if (registrationRequestDto == null)
            {
                return BadRequest("Invalid registration request.");
            }

            var result = await _authService.Register(registrationRequestDto);

            if (!string.IsNullOrEmpty(result))
            {
                return BadRequest(new { Message = result });
            }

            return Ok(new { Message = "Registration successful." });
        }



        //Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto == null)
            {
                return BadRequest("Invalid login request.");
            }

            var loginResponse = await _authService.Login(loginRequestDto);

            if (loginResponse == null || loginResponse.User == null)
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }

            return Ok(loginResponse);
        }





        //Role
        [HttpPost("AssignRole")]
        public async Task<IActionResult> Login([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            if (registrationRequestDto == null)
            {
                return BadRequest("Invalid request.");
            }

            var assignRoleSuccessful = 
                await _authService.AssignRole(registrationRequestDto.Email, registrationRequestDto.RoleName.ToUpper());
           
            if (!assignRoleSuccessful)
            {
                return Unauthorized(new { Message = "Error encountered" });
            }


            return Ok(registrationRequestDto);
        }
    }
}
