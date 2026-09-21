using Microsoft.Extensions.Logging;
using DbContext;
using Configuration;
using Models;
using Seido.Utilities.SeedGenerator;
using Microsoft.EntityFrameworkCore;

namespace DbRepos;

public class AdminDbRepos : IAdminDbRepos
{
    private readonly ILogger<AdminDbRepos> _logger;
    private readonly Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    public async Task SeedAsync(int nrItems)
    {
        var hasRequiredSeedData = await _dbContext.Users.CountAsync() >= 50
            && await _dbContext.Cities.CountAsync() >= 100
            && await _dbContext.Attractions.CountAsync() >= 1000;

        if (hasRequiredSeedData)
        {
            _logger.LogInformation("Database already contains seed data; skipping travel seeding.");
            return;
        }

        if (await _dbContext.Countries.AnyAsync())
            await ClearTestDataAsync();

        var generator = new SeedGenerator();

        var categoryNames = new[]
        {
            "Museum",
            "Restaurant",
            "Park",
            "Beach",
            "Historic",
            "Cafe",
            "Nature",
            "Architecture"
        };

        var categories = categoryNames
            .Select(name => new Category { CategoryId = Guid.NewGuid(), Name = name })
            .ToList();

        _dbContext.Categories.AddRange(categories);

        var countries = new[] { "Sweden", "Norway", "Denmark", "Finland" }
            .Select(name => new Country { CountryId = Guid.NewGuid(), Name = name, Cities = new List<City>() })
            .ToList();

        _dbContext.Countries.AddRange(countries);
        await _dbContext.SaveChangesAsync();

        var cities = new List<City>();
        foreach (var country in countries)
        {
            for (var i = 0; i < 25; i++)
            {
                cities.Add(new City
                {
                    CityId = Guid.NewGuid(),
                    CountryId = country.CountryId,
                    Name = $"{country.Name} City {i + 1}",
                    Country = country,
                    Attractions = new List<Attraction>()
                });
            }
        }

        _dbContext.Cities.AddRange(cities);
        await _dbContext.SaveChangesAsync();

        var streetNames = new[]
        {
            "Main Street", "Harbor Road", "Oak Avenue", "Market Square",
            "River Lane", "Church Street", "Park Boulevard", "Station Road"
        };

        var attractions = new List<Attraction>();
        foreach (var city in cities)
        {
            for (var i = 0; i < 10; i++)
            {
                var categorySet = categories
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(2)
                    .ToList();

                // random street + house number gives each attraction a unique address within the city
                var address = $"{streetNames[generator.Next(0, streetNames.Length)]} {generator.Next(1, 200)}";

                var attraction = new Attraction
                {
                    AttractionId = Guid.NewGuid(),
                    CityId = city.CityId,
                    Name = $"{city.Name} {generator.FromList(new List<string> { "Viewpoint", "Garden", "Market", "Museum", "Harbor", "Square" })} {i + 1}",
                    Description = $"A popular destination in {city.Name} with a strong mix of local culture and travel experiences.",
                    Address = address,
                    CreatedAt = generator.DateAndTime(2023, 2025),
                    City = city,
                    Categories = categorySet,
                    Reviews = new List<Review>()
                };

                attractions.Add(attraction);
            }
        }

        _dbContext.Attractions.AddRange(attractions);
        await _dbContext.SaveChangesAsync();

        var users = new List<User>();
        for (var i = 0; i < 50; i++)
        {
            var firstName = generator.FirstName;
            var lastName = generator.LastName;
            users.Add(new User
            {
                UserId = Guid.NewGuid(),
                Username = $"{firstName}.{lastName}".ToLowerInvariant(),
                Email = generator.Email(firstName, lastName),
                CreatedAt = generator.DateAndTime(2022, 2025),
                Reviews = new List<Review>()
            });
        }

        _dbContext.Users.AddRange(users);
        await _dbContext.SaveChangesAsync();

        var reviews = new List<Review>();
        foreach (var attraction in attractions)
        {
            var reviewCount = generator.Next(0, 21);
            for (var i = 0; i < reviewCount; i++)
            {
                var user = users[(i + attraction.GetHashCode()) % users.Count];
                reviews.Add(new Review
                {
                    ReviewId = Guid.NewGuid(),
                    AttractionId = attraction.AttractionId,
                    UserId = user.UserId,
                    CommentText = $"I really enjoyed the atmosphere and the local recommendations around {attraction.Name}.",
                    Score = (byte)(generator.Next(3, 6) + 1),
                    CreatedAt = generator.DateAndTime(2024, 2025),
                    Attraction = attraction,
                    User = user
                });
            }
        }

        _dbContext.Reviews.AddRange(reviews);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Seeded {CountryCount} countries, {CityCount} cities, {AttractionCount} attractions, {ReviewCount} reviews and {UserCount} users.",
            countries.Count,
            cities.Count,
            attractions.Count,
            reviews.Count,
            users.Count);
    }

    public Task ClearTestDataAsync()
        => ClearTestDataInternalAsync();

    public async Task<DatabaseOverview> GetOverviewAsync()
    {
        await EnsureSqlServerObjectsAsync();
        return await _dbContext.Database.SqlQueryRaw<DatabaseOverview>(
                "SELECT UserCount, CityCount, AttractionCount FROM dbo.TravelDatabaseOverview")
            .SingleAsync();
    }

    private async Task ClearTestDataInternalAsync()
    {
        await EnsureSqlServerObjectsAsync();
        await _dbContext.Database.ExecuteSqlRawAsync("EXEC dbo.ClearTravelTestData");
    }

    private async Task EnsureSqlServerObjectsAsync()
    {
        if (!_dbContext.Database.IsSqlServer())
            throw new NotSupportedException("ClearTestData and Overview currently require SQL Server.");

        await _dbContext.Database.ExecuteSqlRawAsync("""
            CREATE OR ALTER PROCEDURE dbo.ClearTravelTestData
            AS
            BEGIN
                SET NOCOUNT ON;
                DELETE FROM dbo.AttractionCategories;
                DELETE FROM dbo.Reviews;
                DELETE FROM dbo.Attractions;
                DELETE FROM dbo.Cities;
                DELETE FROM dbo.Categories;
                DELETE FROM dbo.Users;
                DELETE FROM dbo.Countries;
            END
            """);

        await _dbContext.Database.ExecuteSqlRawAsync("""
            CREATE OR ALTER VIEW dbo.TravelDatabaseOverview
            AS
            SELECT
                (SELECT COUNT(*) FROM dbo.Users) AS UserCount,
                (SELECT COUNT(*) FROM dbo.Cities) AS CityCount,
                (SELECT COUNT(*) FROM dbo.Attractions) AS AttractionCount
            """);
    }

    public AdminDbRepos(ILogger<AdminDbRepos> logger, Encryptions encryptions, MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
}

public class DatabaseOverview
{
    public int UserCount { get; set; }
    public int CityCount { get; set; }
    public int AttractionCount { get; set; }
}
