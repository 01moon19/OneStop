
using System.Net;
using System.Net.Http.Json;
using ECommerce.OrderService.Clients.DTOs;
using ECommerce.OrderService.Clients.Interfaces;


namespace ECommerce.OrderService.Clients;

public class ProductServiceClient : IProductServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ProductServiceClient(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<ProductResponseDTO?> GetProductByIdAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(
            $"api/products/{productId}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<ProductResponseDTO>(
                cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException(
                "Product Service returned an empty response.");
    }

    public async Task<bool> DeductStockAsync(
        IReadOnlyCollection<InventoryItemDTO> items,
        CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["InternalApi:Key"]
            ?? throw new InvalidOperationException(
                "Internal API key is missing.");

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "api/internal/inventory/deduct");

        request.Headers.Add("X-Internal-Api-Key", apiKey);
        request.Content = JsonContent.Create(new { Items = items });

        using var response = await _httpClient.SendAsync(
            request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.Conflict)
            return false;

        response.EnsureSuccessStatusCode();

        return true;
    }
}
