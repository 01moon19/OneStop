namespace ECommerce.OrderService.Models;

public class Order
{
    public enum OrderStatus
    {
        pending,
        completed
    }

    public int OrderId { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus orderStatus { get; set; } = OrderStatus.pending;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}