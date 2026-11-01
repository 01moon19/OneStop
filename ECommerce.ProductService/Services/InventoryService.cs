
using ECommerce.ProductService.Data;
using ECommerce.ProductService.DTOs.Inventory;
using ECommerce.ProductService.Models.DTOs.Inventory;
using ECommerce.ProductService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ProductService.Services;

public class InventoryService : IInventoryService
{
    private readonly ProductDbContext _context;

    public InventoryService(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DeductStockAsync(
        IReadOnlyCollection<InventoryItemDTO> items)
    {
        if (items == null ||
            items.Count == 0 ||
            items.Any(i => i.ProductId <= 0 || i.Quantity <= 0) ||
            items.GroupBy(i => i.ProductId).Any(g => g.Count() > 1))
        {
            throw new ArgumentException("Invalid inventory request.");
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        foreach (var item in items.OrderBy(i => i.ProductId))
        {
            var affectedRows = await _context.Products
                .Where(p =>
                    p.ProductId == item.ProductId &&
                    p.StockQuantity >= item.Quantity)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(
                        p => p.StockQuantity,
                        p => p.StockQuantity - item.Quantity));

            if (affectedRows != 1)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        await transaction.CommitAsync();
        return true;
    }
}
