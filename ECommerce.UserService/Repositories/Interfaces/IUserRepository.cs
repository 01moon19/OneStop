using ECommerce.UserService.Models;

namespace ECommerce.UserService.Repositories.Interfaces;

public interface IUserRepository
{
    Task<bool> IsUserExistsbyEmailAsync(string email);

    Task<User?> GetUserByEmailAsync(string email);

    Task AddUserAsync(User user);
}