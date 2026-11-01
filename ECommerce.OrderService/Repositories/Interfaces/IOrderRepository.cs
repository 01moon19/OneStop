
using ECommerce.OrderService.Models;

namespace ECommerce.OrderService.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Order> AddOrderAsync(Order order);

    Task<Order?> GetOrderByIdAsync(int orderId);

    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);

    Task ExecuteInTransactionAsync(Func<Task> operation);
}
