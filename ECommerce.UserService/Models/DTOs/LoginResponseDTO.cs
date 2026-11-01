using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.UserService.Models.DTOs
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }

        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

    }
}
