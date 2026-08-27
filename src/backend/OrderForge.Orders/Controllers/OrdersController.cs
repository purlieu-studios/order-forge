using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("orders")]
public sealed class OrdersController(OrdersDbContext database) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Items = request.Items
                .Select(item => new OrderItem
                {
                    Sku = item.Sku.Trim(),
                    Quantity = item.Quantity
                })
                .ToList()
        };

        database.Orders.Add(order);
        await database.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = order.Id },
            new
            {
                order.Id,
                order.Status,
                order.CreatedAt
            });
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(
        long id,
        CancellationToken cancellationToken)
    {
        var order = await database.Orders
            .AsNoTracking()
            .Where(order => order.Id == id)
            .Select(order => new
            {
                order.Id,
                order.Status,
                order.CreatedAt,
                Items = order.Items.Select(item => new
                {
                    item.Id,
                    item.Sku,
                    item.Quantity
                })
            })
            .SingleOrDefaultAsync(cancellationToken);

        return order is null ? NotFound() : Ok(order);
    }
}