
using ECommerce.OrderService.Clients;
using ECommerce.OrderService.Clients.Interfaces;
using ECommerce.OrderService.DTOs.Cart;
using ECommerce.OrderService.Models;
using ECommerce.OrderService.Repositories.Interfaces;
using ECommerce.OrderService.Services.Interfaces;

namespace ECommerce.OrderService.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductServiceClient _productClient;

    public CartService(
        ICartRepository cartRepository,
        IProductServiceClient productClient)
    {
        _cartRepository = cartRepository;
        _productClient = productClient;
    }

    public async Task<CartDTO?> GetCartAsync(int userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
            return null;

        return await MapToCartDTOAsync(cart);
    }

    public async Task<CartDTO> AddProductToCartAsync(
        int userId, int productId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        var product = await _productClient.GetProductByIdAsync(productId);

        if (product == null)
            throw new KeyNotFoundException("Product not found.");

        var cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId
            };

            await _cartRepository.AddCartAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }

        var existingItem = await _cartRepository.GetCartItemAsync(
            cart.CartId, productId);

        var existingQuantity = existingItem?.Quantity ?? 0;
        var newQuantity = existingQuantity + quantity;

        if (newQuantity > product.StockQuantity)
            throw new InvalidOperationException("Insufficient stock.");

        if (existingItem != null)
        {
            existingItem.Quantity = newQuantity;
            _cartRepository.UpdateCartItem(existingItem);
        }
        else
        {
            await _cartRepository.AddCartItemAsync(new CartItem
            {
                CartId = cart.CartId,
                ProductId = productId,
                Quantity = quantity
            });
        }

        await _cartRepository.SaveChangesAsync();

        var updatedCart = await _cartRepository.GetCartByUserIdAsync(userId)
            ?? throw new InvalidOperationException(
                "Unable to retrieve the updated cart.");

        return await MapToCartDTOAsync(updatedCart);
    }

    public async Task<CartDTO> UpdateCartItemAsync(
        int userId, int productId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        var cart = await _cartRepository.GetCartByUserIdAsync(userId)
            ?? throw new KeyNotFoundException("Cart not found.");

        var cartItem = await _cartRepository.GetCartItemAsync(
            cart.CartId, productId)
            ?? throw new KeyNotFoundException("Cart item not found.");

        var product = await _productClient.GetProductByIdAsync(productId)
            ?? throw new KeyNotFoundException("Product not found.");

        if (quantity > product.StockQuantity)
            throw new InvalidOperationException("Insufficient stock.");

        cartItem.Quantity = quantity;
        _cartRepository.UpdateCartItem(cartItem);

        await _cartRepository.SaveChangesAsync();

        var updatedCart = await _cartRepository.GetCartByUserIdAsync(userId)
            ?? throw new InvalidOperationException(
                "Unable to retrieve the updated cart.");

        return await MapToCartDTOAsync(updatedCart);
    }

    public async Task RemoveCartItemAsync(int userId, int productId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId)
            ?? throw new KeyNotFoundException("Cart not found.");

        var cartItem = await _cartRepository.GetCartItemAsync(
            cart.CartId, productId)
            ?? throw new KeyNotFoundException("Cart item not found.");

        _cartRepository.RemoveCartItem(cartItem);
        await _cartRepository.SaveChangesAsync();
    }

    private async Task<CartDTO> MapToCartDTOAsync(Cart cart)
    {
        var items = new List<CartItemDTO>();

        foreach (var item in cart.CartItems)
        {
            var product = await _productClient.GetProductByIdAsync(
                item.ProductId);

            items.Add(new CartItemDTO
            {
                ProductId = item.ProductId,
                ProductName = product?.Name ?? "Unavailable product",
                UnitPrice = product?.Price ?? 0,
                Quantity = item.Quantity
            });
        }

        return new CartDTO
        {
            CartId = cart.CartId,
            Items = items
        };
    }
}
