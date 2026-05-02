namespace MyWineCellar.Data.Models;

public class Wine
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Varietal { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public WineType Type { get; set; }

    public ICollection<CatalogueEntry> CatalogueEntries { get; set; } = [];
}

public enum WineType
{
    Red,
    White,
    Rose,
    Sparkling,
    Dessert,
    Fortified
}
