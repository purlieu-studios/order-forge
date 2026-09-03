[ApiController]
[Route("inventory")]
public sealed class InventoryController(InventoryDbContext database) : ControllerBase
{
    [HttpPost("stock")]
    public async Task<IActionResult> AddStock(
        AddStockRequest request,
        CancellationToken cancellationToken)
    {
        var sku = request.Sku.Trim();
        var item = await database.InventoryItems
            .SingleOrDefaultAsync(item => item.Sku == sku, cancellationToken);

        if (item is null)
        {
            item = new InventoryItem
            {
                Sku = sku,
                QuantityOnHand = request.Quantity
            };

            database.InventoryItems.Add(item);
        }
        else
        {
            item.QuantityOnHand += request.Quantity;
        }

        await database.SaveChangesAsync(cancellationToken);

        return Ok(new
        {
            item.Sku,
            item.QuantityOnHand,
            item.QuantityReserved
        });
    }
}
