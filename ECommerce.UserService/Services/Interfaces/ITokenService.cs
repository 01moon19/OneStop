using ECommerce.UserService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.UserService.Services.Interfaces
{
    public interface ITokenService
    {
        public TokenResult GenerateToken(int userId, User.Role userRole);
    }
}
