using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.UserService.Models
{
    public class TokenResult
    {
        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
