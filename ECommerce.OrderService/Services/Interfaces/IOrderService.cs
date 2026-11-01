
using ECommerce.OrderService.Models.DTOs.Order;

namespace ECommerce.OrderService.Services.Interfaces;

public interface IOrderService
{
    Task<OrderDTO> PlaceOrderAsync(int userId);

    Task<OrderDTO?> GetOrderByIdAsync(int orderId, int userId);

    Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(int userId);
}
