
using ECommerce.ProductService.Models;
using ECommerce.ProductService.Models.DTOs;
using ECommerce.ProductService.Repositories.Interfaces;
using ECommerce.ProductService.Services.Interfaces;

namespace ECommerce.ProductService.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(
        string? category = null)
    {
        var products = await _productRepository
            .GetAllProductsAsync(category);

        return products.Select(MapToDTO).ToList();
    }

    public async Task<ProductDTO?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository
            .GetProductByIdAsync(id);

        return product == null ? null : MapToDTO(product);
    }

    public async Task<ProductDTO> CreateProductAsync(
        CreateProductDTO dto)
    {
        var product = new Product
        {
            ProductName = dto.Name,
            ProductPrice = dto.Price,
            ProductDescription = dto.Description,
            ProductCategory = dto.Category,
            ProductImage = dto.Image,
            StockQuantity = dto.StockQuantity
        };

        await _productRepository.AddProductAsync(product);
        await _productRepository.SaveChangesAsync();

        return MapToDTO(product);
    }

    public async Task<bool> UpdateProductAsync(
        int id, UpdateProductDTO dto)
    {
        var product = await _productRepository
            .GetProductByIdAsync(id);

        if (product == null)
            return false;

        product.ProductName = dto.Name;
        product.ProductPrice = dto.Price;
        product.ProductDescription = dto.Description;
        product.ProductCategory = dto.Category;
        product.ProductImage = dto.Image;
        product.StockQuantity = dto.StockQuantity;

        // EF Core tracks this entity automatically.
        await _productRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepository
            .GetProductByIdAsync(id);

        if (product == null)
            return false;

        _productRepository.DeleteProduct(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }

    private static ProductDTO MapToDTO(Product product)
    {
        return new ProductDTO
        {
            Id = product.ProductId,
            Name = product.ProductName,
            Price = product.ProductPrice,
            Description = product.ProductDescription,
            Category = product.ProductCategory,
            Image = product.ProductImage,
            StockQuantity = product.StockQuantity
        };
    }
}
