
using ECommerce.ProductService.DTOs.Inventory;
using ECommerce.ProductService.Models.DTOs.Inventory;

namespace ECommerce.ProductService.Services.Interfaces;

public interface IInventoryService
{
    Task<bool> DeductStockAsync(
        IReadOnlyCollection<InventoryItemDTO> items);
}
