
using ECommerce.ProductService.Models.DTOs.Inventory;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.ProductService.DTOs.Inventory;

public class DeductInventoryRequestDTO
{
    [Required]
    [MinLength(1)]
    public List<InventoryItemDTO> Items { get; set; } = new();
}
