
using System.Security.Cryptography;
using System.Text;
using ECommerce.ProductService.DTOs.Inventory;
using ECommerce.ProductService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.ProductService.Controllers;

[ApiController]
[Route("api/internal/inventory")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly IConfiguration _configuration;

    public InventoryController(
        IInventoryService inventoryService,
        IConfiguration configuration)
    {
        _inventoryService = inventoryService;
        _configuration = configuration;
    }

    [HttpPost("deduct")]
    public async Task<IActionResult> DeductStock(
        [FromBody] DeductInventoryRequestDTO request)
    {
        var expectedKey = _configuration["InternalApi:Key"];

        if (string.IsNullOrWhiteSpace(expectedKey))
            return StatusCode(503, new
            {
                message = "Internal authentication is not configured."
            });

        var suppliedKey =
            Request.Headers["X-Internal-Api-Key"].ToString();

        var expectedBytes = Encoding.UTF8.GetBytes(expectedKey);
        var suppliedBytes = Encoding.UTF8.GetBytes(suppliedKey);

        if (expectedBytes.Length != suppliedBytes.Length ||
            !CryptographicOperations.FixedTimeEquals(
                expectedBytes, suppliedBytes))
        {
            return Unauthorized();
        }

        try
        {
            var success = await _inventoryService.DeductStockAsync(
                request.Items);

            if (!success)
                return Conflict(new
                {
                    message = "Insufficient stock or product unavailable."
                });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
