using Microsoft.EntityFrameworkCore;
using MyWineCellar.Data.Models;

namespace MyWineCellar.Data;

public class WineCellarDbContext(DbContextOptions<WineCellarDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Wine> Wines => Set<Wine>();
    public DbSet<CatalogueEntry> CatalogueEntries => Set<CatalogueEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.Property(u => u.DisplayName).IsRequired().HasMaxLength(150);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Username).IsUnique();
        });

        modelBuilder.Entity<Wine>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Name).IsRequired().HasMaxLength(200);
            entity.Property(w => w.Varietal).HasMaxLength(100);
            entity.Property(w => w.Region).HasMaxLength(150);
            entity.Property(w => w.Country).HasMaxLength(100);
            entity.Property(w => w.Type).HasConversion<string>();
        });

        modelBuilder.Entity<CatalogueEntry>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.PurchasePrice).HasPrecision(10, 2);
            entity.HasOne(c => c.User)
                  .WithMany(u => u.CatalogueEntries)
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Wine)
                  .WithMany(w => w.CatalogueEntries)
                  .HasForeignKey(c => c.WineId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "alice", Email = "alice@example.com", DisplayName = "Alice Dupont", CreatedAt = new DateTime(2024, 1, 10) },
            new User { Id = 2, Username = "bob", Email = "bob@example.com", DisplayName = "Bob Martin", CreatedAt = new DateTime(2024, 2, 15) }
        );

        modelBuilder.Entity<Wine>().HasData(
            new Wine { Id = 1, Name = "Château Margaux", Varietal = "Cabernet Sauvignon", Region = "Margaux", Country = "France", Type = WineType.Red },
            new Wine { Id = 2, Name = "Opus One", Varietal = "Cabernet Sauvignon", Region = "Napa Valley", Country = "USA", Type = WineType.Red },
            new Wine { Id = 3, Name = "Cloudy Bay Sauvignon Blanc", Varietal = "Sauvignon Blanc", Region = "Marlborough", Country = "New Zealand", Type = WineType.White },
            new Wine { Id = 4, Name = "Moët & Chandon Brut Impérial", Varietal = "Blend", Region = "Champagne", Country = "France", Type = WineType.Sparkling },
            new Wine { Id = 5, Name = "Penfolds Grange", Varietal = "Shiraz", Region = "South Australia", Country = "Australia", Type = WineType.Red }
        );

        modelBuilder.Entity<CatalogueEntry>().HasData(
            new CatalogueEntry { Id = 1, UserId = 1, WineId = 1, Vintage = 2018, Quantity = 6, PurchasePrice = 580.00m, Notes = "Anniversary gift", AddedAt = new DateTime(2024, 3, 1) },
            new CatalogueEntry { Id = 2, UserId = 1, WineId = 3, Vintage = 2022, Quantity = 12, PurchasePrice = 22.50m, Notes = "Summer whites", AddedAt = new DateTime(2024, 3, 5) },
            new CatalogueEntry { Id = 3, UserId = 1, WineId = 4, Vintage = 2019, Quantity = 3, PurchasePrice = 55.00m, Notes = "For celebrations", AddedAt = new DateTime(2024, 4, 10) },
            new CatalogueEntry { Id = 4, UserId = 2, WineId = 2, Vintage = 2017, Quantity = 2, PurchasePrice = 320.00m, Notes = "Investment bottles", AddedAt = new DateTime(2024, 2, 20) },
            new CatalogueEntry { Id = 5, UserId = 2, WineId = 5, Vintage = 2016, Quantity = 1, PurchasePrice = 850.00m, Notes = "Special occasion", AddedAt = new DateTime(2024, 3, 18) }
        );
    }
}
