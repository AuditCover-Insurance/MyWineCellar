using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyWineCellar.Data.Models;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    [MaxLength(150)]
    public string DisplayName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public ICollection<CellarEntry> CatalogueEntries { get; set; } = [];
}
