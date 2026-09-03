using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("inventory")]
public sealed class InventoryController(IHttpClientFactory httpClientFactory) : ControllerBase
{
    [HttpPost("stock")]
    public async Task<IActionResult> AddStock(
        AddStockRequest request,
        CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("Inventory");

        using var response = await client.PostAsJsonAsync(
            "inventory/stock",
            request,
            cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return new ContentResult
        {
            StatusCode = (int)response.StatusCode,
            ContentType = response.Content.Headers.ContentType?.MediaType ?? "application/json",
            Content = content
        };
    }
}
