namespace MyWineCellar.Models;

public record NewWineRequest(int WineId, int Vintage, int Quantity, decimal? PurchasePrice, string? Notes, int? MaturityYear);

public record NewCustomWineRequest(string Name, string Varietal, string Region, string Country, WineType Type);

public record WineDto(int Id, string Name, string Varietal, string Region, string Country, WineType Type);

public record CellarEntryDto(int Id, int WineId, int Vintage, int Quantity, int? MaturityYear, decimal? PurchasePrice, string? Notes, DateTime AddedAt, WineDto Wine);

public enum WineType
{
    Red,
    White,
    Rose,
    Sparkling,
    Dessert,
    Fortified
}