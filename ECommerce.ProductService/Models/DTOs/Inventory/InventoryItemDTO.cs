using System.ComponentModel.DataAnnotations;

namespace ECommerce.ProductService.Models.DTOs.Inventory;

public class InventoryItemDTO
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
    