using System.ComponentModel.DataAnnotations;
using MyWineCellar.Models;

namespace MyWineCellar.Data.Models;

public class Wine
{
    public int Id { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public ICollection<CellarEntry> CatalogueEntries { get; set; } = [];

    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(100)]
    public string Varietal { get; set; } = string.Empty;
    [MaxLength(150)]
    public string Region { get; set; } = string.Empty;
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;
    public WineType Type { get; set; }
}