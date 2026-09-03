using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("orders")]
public sealed class OrdersController(IHttpClientFactory httpClientFactory) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] JsonElement request,
        CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("Orders");

        using var response = await client.PostAsJsonAsync(
            "orders",
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
