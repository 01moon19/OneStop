
using ECommerce.OrderService.Clients.DTOs;

namespace ECommerce.OrderService.Clients.Interfaces;

public interface IProductServiceClient
{
    Task<ProductResponseDTO?> GetProductByIdAsync(
        int productId,
        CancellationToken cancellationToken = default);

    Task<bool> DeductStockAsync(
        IReadOnlyCollection<InventoryItemDTO> items,
        CancellationToken cancellationToken = default);
}
