
namespace ECommerce.OrderService.DTOs.Cart;

public class CartDTO
{
    public int CartId { get; set; }

    public List<CartItemDTO> Items { get; set; } = new();

    public decimal TotalAmount => Items.Sum(item => item.Subtotal);
}
