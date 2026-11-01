
using ECommerce.ProductService.Models.DTOs;
using ECommerce.ProductService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.ProductService.Controllers;

[ApiController]
[Route("api/products")]
[Authorize(Roles = "Admin")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ProductDTO>>>
        GetAllProducts([FromQuery] string? category = null)
    {
        var products = await _productService
            .GetAllProductsAsync(category);

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProductDTO>>
        GetProductById(int id)
    {
        var product = await _productService
            .GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDTO>>
        CreateProduct([FromBody] CreateProductDTO dto)
    {
        var createdProduct = await _productService
            .CreateProductAsync(dto);

        return CreatedAtAction(
            nameof(GetProductById),
            new { id = createdProduct.Id },
            createdProduct);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(
        int id,
        [FromBody] UpdateProductDTO dto)
    {
        var updated = await _productService
            .UpdateProductAsync(id, dto);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _productService
            .DeleteProductAsync(id);

        return deleted ? NoContent() : NotFound();
    }
}
