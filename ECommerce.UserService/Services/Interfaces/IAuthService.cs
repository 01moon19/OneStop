
using ECommerce.UserService.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.UserService.Services.Interfaces
{
    public interface IAuthService
    {

        public Task<UserDTO> RegisterUserAsync(RegisterUserDTO registerUserDTO);

        public Task<LoginResponseDTO> LoginUserAsync(LoginUserDTO loginUserDTO);
    }
}
