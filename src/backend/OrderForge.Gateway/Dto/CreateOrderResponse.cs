public sealed class CreateOrderResponse
{
    public long Id { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
}
