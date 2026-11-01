
using ECommerce.OrderService.Models;

namespace ECommerce.OrderService.Repositories.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserIdAsync(int userId);

    Task<CartItem?> GetCartItemAsync(int cartId, int productId);

    Task AddCartAsync(Cart cart);

    Task AddCartItemAsync(CartItem item);

    void UpdateCartItem(CartItem item);

    void RemoveCartItem(CartItem item);

    Task ClearCartAsync(int cartId);

    Task SaveChangesAsync();
}
