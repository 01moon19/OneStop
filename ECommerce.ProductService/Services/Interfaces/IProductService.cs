
using ECommerce.ProductService.Models.DTOs;

namespace ECommerce.ProductService.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync(
        string? category = null);

    Task<ProductDTO?> GetProductByIdAsync(int id);

    Task<ProductDTO> CreateProductAsync(
        CreateProductDTO productDto);

    Task<bool> UpdateProductAsync(
        int id, UpdateProductDTO productDto);

    Task<bool> DeleteProductAsync(int id);
}
