using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("orders")]
public sealed class OrdersController(IHttpClientFactory httpClientFactory) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("Orders");

        using var response = await client.PostAsJsonAsync(
            "orders",
            request,
            cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<CreateOrderResponse>(
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

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(
        long id,
        CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("Orders");

        using var response = await client.GetAsync(
            $"orders/{id}",
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
