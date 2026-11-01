
using ECommerce.OrderService.Clients;
using ECommerce.OrderService.Clients.DTOs;
using ECommerce.OrderService.Clients.Interfaces;
using ECommerce.OrderService.Models;
using ECommerce.OrderService.Models.DTOs.Order;
using ECommerce.OrderService.Repositories.Interfaces;
using ECommerce.OrderService.Services.Interfaces;

namespace ECommerce.OrderService.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductServiceClient _productClient;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IProductServiceClient productClient)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _productClient = productClient;
    }

    public async Task<OrderDTO> PlaceOrderAsync(int userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null || !cart.CartItems.Any())
            throw new InvalidOperationException("Your cart is empty.");

        var orderItems = new List<OrderItem>();
        var inventoryItems = new List<InventoryItemDTO>();

        foreach (var cartItem in cart.CartItems)
        {
            if (cartItem.Quantity <= 0)
                throw new ArgumentException(
                    "Cart quantity must be greater than zero.");

            var product = await _productClient.GetProductByIdAsync(
                cartItem.ProductId);

            if (product == null)
                throw new KeyNotFoundException(
                    $"Product {cartItem.ProductId} not found.");

            if (cartItem.Quantity > product.StockQuantity)
                throw new InvalidOperationException(
                    $"Insufficient stock for {product.Name}.");

            orderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = cartItem.Quantity,
                UnitPrice = product.Price
            });

            inventoryItems.Add(new InventoryItemDTO
            {
                ProductId = product.Id,
                Quantity = cartItem.Quantity
            });
        }

        // ProductDB inventory deduction is a separate transaction.
        var deducted = await _productClient.DeductStockAsync(
            inventoryItems);

        if (!deducted)
            throw new InvalidOperationException(
                "Insufficient stock to complete the order.");

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = orderItems.Sum(
                i => i.UnitPrice * i.Quantity),
            orderStatus = Order.OrderStatus.completed,
            OrderItems = orderItems
        };

        // Order creation and cart clearing share the OrderDB transaction.
        await _orderRepository.ExecuteInTransactionAsync(async () =>
        {
            await _orderRepository.AddOrderAsync(order);
            await _cartRepository.ClearCartAsync(cart.CartId);
        });

        return MapToOrderDTO(order);
    }

    public async Task<OrderDTO?> GetOrderByIdAsync(
        int orderId, int userId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);

        if (order == null || order.UserId != userId)
            return null;

        return MapToOrderDTO(order);
    }

    public async Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(
        int userId)
    {
        var orders = await _orderRepository
            .GetOrdersByUserIdAsync(userId);

        return orders.Select(MapToOrderDTO).ToList();
    }

    private static OrderDTO MapToOrderDTO(Order order)
    {
        return new OrderDTO
        {
            OrderId = order.OrderId,
            OrderDate = order.OrderDate,
            OrderStatus = order.orderStatus,
            TotalAmount = order.TotalAmount,

            OrderItems = order.OrderItems
                .Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                })
                .ToList()
        };
    }
}
