
using ECommerce.OrderService.DTOs.Cart;

namespace ECommerce.OrderService.Services.Interfaces;

public interface ICartService
{
    Task<CartDTO?> GetCartAsync(int userId);

    Task<CartDTO> AddProductToCartAsync(
        int userId, int productId, int quantity);

    Task<CartDTO> UpdateCartItemAsync(
        int userId, int productId, int quantity);

    Task RemoveCartItemAsync(int userId, int productId);
}
