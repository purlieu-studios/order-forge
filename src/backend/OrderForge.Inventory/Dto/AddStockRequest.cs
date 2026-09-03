public sealed class AddStockRequest
{
    [Required]
    [MaxLength(64)]
    public string Sku { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
