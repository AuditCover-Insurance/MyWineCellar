using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyWineCellar.Data;
using MyWineCellar.Data.Models;
using MyWineCellar.Models;
using System.Security.Claims;

namespace MyWineCellar.Service;

public class WineService(WineCellarDbContext db, IHttpContextAccessor httpContextAccessor) : BaseService(db, httpContextAccessor)
{
    public async Task<IEnumerable<CellarEntryDto>> GetCellarEntriesAsync(int userId, int? addedSinceInMonths)
    {
        return await Db.CellarEntries
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .Where(e => addedSinceInMonths == null || e.AddedAt >= DateTime.UtcNow.AddMonths(-addedSinceInMonths.Value))
            .Select(e => new CellarEntryDto(
                e.Id,
                e.WineId,
                e.Vintage,
                e.Quantity,
                e.MaturityYear,
                e.PurchasePrice,
                e.Notes,
                e.AddedAt,
                new WineDto(e.Wine.Id, e.Wine.Name, e.Wine.Varietal, e.Wine.Region, e.Wine.Country, e.Wine.Type)
            ))
            .ToListAsync();
    }

    public async Task<IEnumerable<WineDto>> GetPublicWinesAsync()
    {
        return await Db.Wines
            .Where(w => w.UserId == null)
            .Select(w => new WineDto(w.Id, w.Name, w.Varietal, w.Region, w.Country, w.Type))
            .ToListAsync();
    }

    public async Task<IEnumerable<WineDto>> GetWinesByCountryAsync(string country)
    {
        return await Db.Wines
            .AsNoTracking()
            .Where(w => w.UserId == null && w.Country == country)
            .Select(w => new WineDto(w.Id, w.Name, w.Varietal, w.Region, w.Country, w.Type))
            .ToListAsync();
    }

    public async Task<IEnumerable<WineDto>> GetCustomWinesAsync(int userId)
    {
        return await Db.Wines
            .Where(w => w.UserId == userId)
            .Select(w => new WineDto(w.Id, w.Name, w.Varietal, w.Region, w.Country, w.Type))
            .ToListAsync();
    }

    public async Task<WineDto> AddCustomWineAsync(int userId, NewCustomWineRequest request)
    {
        var wine = new Wine
        {
            UserId = userId,
            Name = request.Name,
            Varietal = request.Varietal,
            Region = request.Region,
            Country = request.Country,
            Type = request.Type
        };

        Db.Wines.Add(wine);
        await Db.SaveChangesAsync();

        return new WineDto(wine.Id, wine.Name, wine.Varietal, wine.Region, wine.Country, request.Type);
    }

    public async Task<CellarEntryDto> AddWineToCellarAsync(int userId, NewWineRequest request)
    {
        var entry = new CellarEntry
        {
            UserId = userId,
            WineId = request.WineId,
            Vintage = request.Vintage,
            Quantity = request.Quantity,
            MaturityYear = request.MaturityYear,
            PurchasePrice = request.PurchasePrice,
            Notes = request.Notes,
            AddedAt = DateTime.UtcNow
        };

        Db.CellarEntries.Add(entry);
        await Db.SaveChangesAsync();

        await Db.Entry(entry).Reference(e => e.Wine).LoadAsync();

        return new CellarEntryDto(
            entry.Id,
            entry.WineId,
            entry.Vintage,
            entry.Quantity,
            entry.MaturityYear,
            entry.PurchasePrice,
            entry.Notes,
            entry.AddedAt,
            new WineDto(entry.Wine.Id, entry.Wine.Name, entry.Wine.Varietal, entry.Wine.Region, entry.Wine.Country, entry.Wine.Type)
        );
    }
}
