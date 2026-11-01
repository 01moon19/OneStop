
using ECommerce.OrderService.Models.DTOs.Cart;
using ECommerce.OrderService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerce.OrderService.Controllers;

[ApiController]
[Route("api/cart")]
[Authorize(Roles = "Customer")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private int GetUserId()
    {
        var userIdClaim =
            User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException(
                "Invalid user identity.");

        return userId;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var cart = await _cartService.GetCartAsync(GetUserId());

        if (cart == null)
            return NotFound(new { message = "Cart not found." });

        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddProductToCart(
        [FromBody] AddToCartDTO request)
    {
        try
        {
            var cart = await _cartService.AddProductToCartAsync(
                GetUserId(),
                request.ProductId,
                request.Quantity);

            return Ok(cart);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("items/{productId:int}")]
    public async Task<IActionResult> UpdateCartItem(
        int productId,
        [FromQuery] int quantity)
    {
        try
        {
            var cart = await _cartService.UpdateCartItemAsync(
                GetUserId(), productId, quantity);

            return Ok(cart);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("items/{productId:int}")]
    public async Task<IActionResult> RemoveCartItem(int productId)
    {
        try
        {
            await _cartService.RemoveCartItemAsync(
                GetUserId(), productId);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
