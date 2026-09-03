public sealed class AddStockResponse
{
    public string Sku { get; set; } = string.Empty;

    public int QuantityOnHand { get; set; }

    public int QuantityReserved { get; set; }
}
