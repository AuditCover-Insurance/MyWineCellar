using Microsoft.EntityFrameworkCore;
using MyWineCellar.Data.Models;
using MyWineCellar.Models;

namespace MyWineCellar.Data;

public class WineCellarDbContext(DbContextOptions<WineCellarDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Wine> Wines => Set<Wine>();
    public DbSet<CellarEntry> CellarEntries => Set<CellarEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Wine>()
            .Property(w => w.Type).HasConversion<string>();

        modelBuilder.Entity<Wine>()
            .HasOne(w => w.User)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<CellarEntry>()
            .HasOne(c => c.Wine)
            .WithMany(w => w.CatalogueEntries)
            .OnDelete(DeleteBehavior.Restrict);

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "alice", Email = "alice@example.com", DisplayName = "Alice Dupont", Password = "alice", CreatedAt = new DateTime(2024, 1, 10) },
            new User { Id = 2, Username = "bob", Email = "bob@example.com", DisplayName = "Bob Martin", Password = "bob", CreatedAt = new DateTime(2024, 2, 15) }
        );

        modelBuilder.Entity<Wine>().HasData(
            new Wine { Id = 1, Name = "Château Margaux", Varietal = "Cabernet Sauvignon", Region = "Margaux", Country = "France", Type = WineType.Red },
            new Wine { Id = 2, Name = "Opus One", Varietal = "Cabernet Sauvignon", Region = "Napa Valley", Country = "USA", Type = WineType.Red },
            new Wine { Id = 3, Name = "Cloudy Bay Sauvignon Blanc", Varietal = "Sauvignon Blanc", Region = "Marlborough", Country = "New Zealand", Type = WineType.White },
            new Wine { Id = 4, Name = "Moët & Chandon Brut Impérial", Varietal = "Blend", Region = "Champagne", Country = "France", Type = WineType.Sparkling },
            new Wine { Id = 5, Name = "Penfolds Grange", Varietal = "Shiraz", Region = "South Australia", Country = "Australia", Type = WineType.Red },
            new Wine { Id = 6, Name = "Alice's Homemade Rosé", Varietal = "Grenache", Region = "Provence", Country = "France", Type = WineType.Rose, UserId = 1 }
        );

        modelBuilder.Entity<CellarEntry>().HasData(
            new CellarEntry { Id = 1, UserId = 1, WineId = 1, Vintage = 2018, Quantity = 6, MaturityYear = 2030, PurchasePrice = 580.00m, Notes = "Anniversary gift", AddedAt = new DateTime(2024, 3, 1) },
            new CellarEntry { Id = 2, UserId = 1, WineId = 3, Vintage = 2022, Quantity = 12, PurchasePrice = 22.50m, Notes = "Summer whites", AddedAt = new DateTime(2024, 3, 5) },
            new CellarEntry { Id = 3, UserId = 1, WineId = 4, Vintage = 2019, Quantity = 3, PurchasePrice = 55.00m, Notes = "For celebrations", AddedAt = new DateTime(2024, 4, 10) },
            new CellarEntry { Id = 4, UserId = 2, WineId = 2, Vintage = 2017, Quantity = 2, MaturityYear = 2028, PurchasePrice = 320.00m, Notes = "Investment bottles", AddedAt = new DateTime(2024, 2, 20) },
            new CellarEntry { Id = 5, UserId = 2, WineId = 5, Vintage = 2016, Quantity = 1, MaturityYear = 2035, PurchasePrice = 850.00m, Notes = "Special occasion", AddedAt = new DateTime(2024, 3, 18) }
        );
    }
}
