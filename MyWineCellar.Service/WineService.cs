using Microsoft.EntityFrameworkCore;
using MyWineCellar.Data;
using MyWineCellar.Data.Models;

namespace MyWineCellar.Service;

public class WineService(WineCellarDbContext db)
{
    public async Task<IEnumerable<Wine>> GetWinesByUserIdAsync(int userId)
    {
        return await db.CatalogueEntries
            .Where(e => e.UserId == userId)
            .Select(e => e.Wine)
            .Distinct()
            .ToListAsync();
    }
}
