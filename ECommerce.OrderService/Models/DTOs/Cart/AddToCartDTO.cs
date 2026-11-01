using System.ComponentModel.DataAnnotations;

namespace ECommerce.OrderService.Models.DTOs.Cart;

public class AddToCartDTO
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;
}
