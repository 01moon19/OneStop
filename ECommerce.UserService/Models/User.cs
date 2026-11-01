
using System.ComponentModel.DataAnnotations;

namespace ECommerce.UserService.Models;

public class User
{
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(254)]
    public string UserEmail { get; set; } = string.Empty;

    public enum Role
    {
        Customer = 0,
        Admin = 1
    }

    public Role UserRole { get; set; } = Role.Customer;
}
