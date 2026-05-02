namespace MyWineCellar.Data.Models;

public class CatalogueEntry
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int WineId { get; set; }
    public int Vintage { get; set; }
    public int Quantity { get; set; }
    public decimal? PurchasePrice { get; set; }
    public string? Notes { get; set; }
    public DateTime AddedAt { get; set; }

    public User User { get; set; } = null!;
    public Wine Wine { get; set; } = null!;
}
