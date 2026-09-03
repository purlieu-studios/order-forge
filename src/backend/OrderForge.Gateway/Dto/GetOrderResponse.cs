public sealed class GetOrderResponse
{
    public long Id { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public List<GetOrderItemResponse> Items { get; set; } = [];
}

public sealed class GetOrderItemResponse
{
    public long Id { get; set; }

    public string Sku { get; set; } = string.Empty;

    public int Quantity { get; set; }
}
