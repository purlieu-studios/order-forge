[Index(nameof(Sku), IsUnique = true)]
public sealed class InventoryItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [MaxLength(64)]
    public string Sku { get; set; } = string.Empty;

    public int QuantityOnHand { get; set; }

    public int QuantityReserved { get; set; }
}