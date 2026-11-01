
using ECommerce.ProductService.Models;

namespace ECommerce.ProductService.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProductsAsync(
        string? category = null);

    Task<Product?> GetProductByIdAsync(int id);

    Task AddProductAsync(Product product);

    void UpdateProduct(Product product);

    void DeleteProduct(Product product);

    Task SaveChangesAsync();
}
