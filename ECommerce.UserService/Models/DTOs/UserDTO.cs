using static ECommerce.UserService.Models.User;

namespace ECommerce.UserService.Models.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        //public string Password { get; set; }
        public string UserEmail { get; set; }
        public Role UserRole { get; set; }
    }
}
