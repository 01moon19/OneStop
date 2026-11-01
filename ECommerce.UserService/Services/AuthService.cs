
using ECommerce.UserService.Exceptions;
using ECommerce.UserService.Models;
using ECommerce.UserService.Models.DTOs;
using ECommerce.UserService.Repositories.Interfaces;
using ECommerce.UserService.Services.Interfaces;
using ECommerce.UserService.Models.DTOs;
using System.Security.Authentication;

namespace ECommerce.UserService.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<UserDTO> RegisterUserAsync(
        RegisterUserDTO dto)
    {
        var email = dto.UserEmail.Trim();

        if (await _userRepository
            .IsUserExistsbyEmailAsync(email))
        {
            throw new DuplicateEmailException(
                "User with this email already exists.");
        }

        var user = new User
        {
            UserName = dto.UserName.Trim(),
            UserEmail = email,
            PasswordHash = BCrypt.Net.BCrypt
                .HashPassword(dto.Password),

            // Public registration cannot assign Admin.
            UserRole = User.Role.Customer
        };

        await _userRepository.AddUserAsync(user);

        return MapToUserDTO(user);
    }

    public async Task<LoginResponseDTO> LoginUserAsync(
        LoginUserDTO dto)
    {
        var user = await _userRepository
            .GetUserByEmailAsync(dto.Email.Trim());

        if (user == null ||
            !BCrypt.Net.BCrypt.Verify(
                dto.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException(
                "Invalid email or password.");
        }

        var token = _tokenService.GenerateToken(
            user.UserId, user.UserRole);

        return new LoginResponseDTO
        {
            User = MapToUserDTO(user),
            Token = token.Token,
            ExpiresAt = token.ExpiresAt
        };
    }

    private static UserDTO MapToUserDTO(User user)
    {
        return new UserDTO
        {
            UserId = user.UserId,
            UserName = user.UserName,
            UserEmail = user.UserEmail,
            UserRole = user.UserRole
        };
    }
}
