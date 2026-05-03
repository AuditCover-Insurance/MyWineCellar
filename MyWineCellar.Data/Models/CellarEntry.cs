using Microsoft.EntityFrameworkCore;

namespace MyWineCellar.Data.Models;

public class CellarEntry
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int WineId { get; set; }
    public Wine Wine { get; set; } = null!;

    public int Vintage { get; set; }
    public int Quantity { get; set; }
    public int? MaturityYear { get; set; }
    [Precision(10, 2)]
    public decimal? PurchasePrice { get; set; }
    public string? Notes { get; set; }
    public DateTime AddedAt { get; set; }

}
