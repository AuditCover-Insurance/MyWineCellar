using Microsoft.EntityFrameworkCore;
using MyWineCellar.Data;
using MyWineCellar.Data.Models;
using MyWineCellar.Service;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using MyWineCellar.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddDbContext<WineCellarDbContext>(options =>
    options.UseSqlite("Data Source=winecellar.db"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<WineService>();

builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("Basic", null);

builder.Services.AddAuthorization();

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WineCellarDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/wines", async (WineService wineService) =>
{
    var wines = await wineService.GetPublicWinesAsync();
    return Results.Ok(wines);
})
.WithName("GetPublicWines");

app.MapGet("/wines/{country}", async (string country, WineService wineService) =>
{
    var wines = await wineService.GetWinesByCountryAsync(country);
    return Results.Ok(wines);
})
.WithName("GetWinesByCountry");

app.MapGet("/users/{userId}/wines", async (int userId, ClaimsPrincipal user, WineService wineService) =>
{
    var authenticatedUserId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
    if (authenticatedUserId != userId)
        return Results.Forbid();

    var wines = await wineService.GetCustomWinesAsync(userId);
    return Results.Ok(wines);
})
.RequireAuthorization()
.WithName("GetCustomWines");

app.MapPost("/users/{userId}/wines", async (int userId, NewCustomWineRequest request, ClaimsPrincipal user, WineService wineService) =>
{
    var authenticatedUserId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
    if (authenticatedUserId != userId)
        return Results.Forbid();

    var wine = await wineService.AddCustomWineAsync(userId, request);
    return Results.Created($"/users/{userId}/wines/{wine.Id}", wine);
})
.RequireAuthorization()
.WithName("AddCustomWine");

app.MapGet("/users/{userId}/cellar", async (int userId, int? olderThanMonths, ClaimsPrincipal user, WineService wineService) =>
{
    var wines = await wineService.GetCellarEntriesAsync(userId, olderThanMonths);
    return Results.Ok(wines);
})
.RequireAuthorization()
.WithName("GetCellarEntries");

app.MapPost("users/{userId}/cellar", async (int userId, NewWineRequest request, WineService wineService) =>
{
    var entry = await wineService.AddWineToCellarAsync(userId, request);
    return Results.Created($"users/{userId}/cellar/{entry.Id}", entry);
})
.RequireAuthorization()
.WithName("AddCellarEntry");

app.Run();


