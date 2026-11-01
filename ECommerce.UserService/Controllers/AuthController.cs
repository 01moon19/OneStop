using ECommerce.UserService.Exceptions;
using ECommerce.UserService.Models.DTOs;
using ECommerce.UserService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace ECommerce.UserService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase //we're using controllerbase because it is restapi without mvc view support
    {
        private readonly IAuthService _authservice;

        public AuthController(IAuthService authservice)
        {
            _authservice = authservice;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> RegisterUser([FromBody] RegisterUserDTO registerUserDTO)
        {
            try
            {
                var user = await _authservice.RegisterUserAsync(registerUserDTO);
                return StatusCode(201, user);
            }
            catch (DuplicateEmailException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> LoginUserAsync([FromBody] LoginUserDTO loginUserDTO)
        {
            try
            {
                var user = await _authservice.LoginUserAsync(loginUserDTO);
                return Ok(user);
            }
            catch (InvalidCredentialsException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
