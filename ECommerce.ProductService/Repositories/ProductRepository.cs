
using ECommerce.ProductService.Data;
using ECommerce.ProductService.Models;
using ECommerce.ProductService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ProductService.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _context;

    public ProductRepository(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(
        string? category = null)
    {
        var query = _context.Products
            .AsNoTracking()
            .Where(p => p.StockQuantity > 0);

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p =>
                p.ProductCategory == category);
        }

        return await query.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public void UpdateProduct(Product product)
    {
        _context.Products.Update(product);
    }

    public void DeleteProduct(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
