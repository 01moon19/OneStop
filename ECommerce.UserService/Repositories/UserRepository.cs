
using ECommerce.UserService.Data;
using ECommerce.UserService.Models;
using ECommerce.UserService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.UserService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUserExistsbyEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException(
                "Email cannot be empty.", nameof(email));

        return await _context.Users
            .AnyAsync(u => u.UserEmail == email);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException(
                "Email cannot be empty.", nameof(email));

        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserEmail == email);
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
