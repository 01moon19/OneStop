
using System.ComponentModel.DataAnnotations;

namespace ECommerce.UserService.Models.DTOs;

public class RegisterUserDTO
{
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(254)]
    public string UserEmail { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
}
