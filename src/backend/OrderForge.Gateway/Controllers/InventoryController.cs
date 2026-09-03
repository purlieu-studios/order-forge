using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("inventory")]
public sealed class InventoryController(IHttpClientFactory httpClientFactory) : ControllerBase
{
    [HttpPost("stock")]
    [ProducesResponseType(typeof(AddStockResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddStock(
        AddStockRequest request,
        CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("Inventory");

        using var response = await client.PostAsJsonAsync(
            "inventory/stock",
            request,
            cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AddStockResponse>(
                cancellationToken);

            return StatusCode((int)response.StatusCode, result);
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return new ContentResult
        {
            StatusCode = (int)response.StatusCode,
            ContentType = response.Content.Headers.ContentType?.MediaType ?? "application/json",
            Content = content
        };
    }
}
