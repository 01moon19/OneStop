
using OrderEntity = ECommerce.OrderService.Models.Order;

namespace ECommerce.OrderService.Models.DTOs.Order;

public class OrderDTO
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public OrderEntity.OrderStatus OrderStatus { get; set; }

    public decimal TotalAmount { get; set; }

    public List<OrderItemDTO> OrderItems { get; set; } = new();
}
