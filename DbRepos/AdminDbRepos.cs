using Microsoft.Extensions.Logging;
using DbContext;
using Configuration;
using Models;
using DbModels;
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

        var hasRequiredSeedData = await _dbContext.Users.CountAsync() >= 50
            && await _dbContext.Countries.CountAsync() >= 4
            && await _dbContext.Cities.CountAsync() >= 100
            && await _dbContext.Attractions.CountAsync() >= 1000
            && await _dbContext.Categories.CountAsync() >= categoryNames.Length
            && !await _dbContext.Attractions.AnyAsync(attraction => !attraction.Categories.Any())
            && !await _dbContext.Attractions.AnyAsync(attraction => attraction.Reviews.Count > 20);

        if (hasRequiredSeedData)
        {
            _logger.LogInformation("Database already contains seed data; skipping travel seeding.");
            return;
        }

        var hasExistingData = await _dbContext.Countries.AnyAsync()
            || await _dbContext.Cities.AnyAsync()
            || await _dbContext.Attractions.AnyAsync()
            || await _dbContext.Categories.AnyAsync()
            || await _dbContext.Users.AnyAsync()
            || await _dbContext.Reviews.AnyAsync();

        if (hasExistingData)
            await ClearTestDataAsync();

        var strategy = _dbContext.Database.CreateExecutionStrategy();
        var reviewCount = 0;
        await strategy.ExecuteAsync(async () =>
        {
            _dbContext.ChangeTracker.Clear();
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            reviewCount = await SeedTravelDataAsync(categoryNames);
            await transaction.CommitAsync();
        });

        _logger.LogInformation("Seeded 4 countries, 100 cities, 1000 attractions, {ReviewCount} reviews and 50 users.",
            reviewCount);
    }

    private async Task<int> SeedTravelDataAsync(string[] categoryNames)
    {
        var generator = new SeedGenerator();
        var categories = categoryNames
            .Select(name => new CategoryDbM { CategoryId = Guid.NewGuid(), Name = name })
            .ToList();
        var countries = new[] { "Sweden", "Norway", "Denmark", "Finland" }
            .Select(name => new CountryDbM { CountryId = Guid.NewGuid(), Name = name })
            .ToList();

        _dbContext.Categories.AddRange(categories);
        _dbContext.Countries.AddRange(countries);
        await _dbContext.SaveChangesAsync();

        var countryAssignments = countries.ToList();
        while (countryAssignments.Count < 100)
            countryAssignments.Add(countries[generator.Next(0, countries.Count)]);

        for (var index = countryAssignments.Count - 1; index > 0; index--)
        {
            var swapIndex = generator.Next(0, index + 1);
            (countryAssignments[index], countryAssignments[swapIndex]) =
                (countryAssignments[swapIndex], countryAssignments[index]);
        }

        var cities = countryAssignments.Select((country, index) => new CityDbM
        {
            CityId = Guid.NewGuid(),
            CountryId = country.CountryId,
            Name = $"{country.Name} City {index + 1}"
        }).ToList();

        _dbContext.Cities.AddRange(cities);
        await _dbContext.SaveChangesAsync();

        var streetNames = new[]
        {
            "Main Street", "Harbor Road", "Oak Avenue", "Market Square",
            "River Lane", "Church Street", "Park Boulevard", "Station Road"
        };
        var attractionTypes = new[] { "Viewpoint", "Garden", "Market", "Museum", "Harbor", "Square" };
        var attractions = new List<AttractionDbM>(1000);

        for (var index = 0; index < 1000; index++)
        {
            var city = cities[generator.Next(0, cities.Count)];
            var firstCategoryIndex = generator.Next(0, categories.Count);
            var secondCategoryIndex = generator.Next(0, categories.Count - 1);
            if (secondCategoryIndex >= firstCategoryIndex)
                secondCategoryIndex++;

            attractions.Add(new AttractionDbM
            {
                AttractionId = Guid.NewGuid(),
                CityId = city.CityId,
                Name = $"{city.Name} {attractionTypes[generator.Next(0, attractionTypes.Length)]} {index + 1}",
                Description = $"A popular destination in {city.Name} with a strong mix of local culture and travel experiences.",
                Address = $"{streetNames[generator.Next(0, streetNames.Length)]} {generator.Next(1, 200)}",
                CreatedAt = generator.DateAndTime(2023, 2025),
                Categories = new List<Category>
                {
                    categories[firstCategoryIndex],
                    categories[secondCategoryIndex]
                }
            });
        }

        _dbContext.Attractions.AddRange(attractions);
        await _dbContext.SaveChangesAsync();

        var users = new List<UserDbM>();
        for (var index = 0; index < 50; index++)
        {
            var firstName = generator.FirstName;
            var lastName = generator.LastName;
            users.Add(new UserDbM
            {
                UserId = Guid.NewGuid(),
                Username = $"{firstName}.{lastName}".ToLowerInvariant(),
                Email = generator.Email(firstName, lastName),
                CreatedAt = generator.DateAndTime(2022, 2025)
            });
        }

        _dbContext.Users.AddRange(users);
        await _dbContext.SaveChangesAsync();

        var reviews = new List<ReviewDbM>();
        foreach (var attraction in attractions)
        {
            var attractionReviewCount = generator.Next(0, 21);
            for (var index = 0; index < attractionReviewCount; index++)
            {
                var user = users[generator.Next(0, users.Count)];
                reviews.Add(new ReviewDbM
                {
                    ReviewId = Guid.NewGuid(),
                    AttractionId = attraction.AttractionId,
                    UserId = user.UserId,
                    CommentText = $"I really enjoyed the atmosphere and the local recommendations around {attraction.Name}.",
                    Score = generator.Next(1, 6),
                    CreatedAt = generator.DateAndTime(2024, 2025)
                });
            }
        }

        _dbContext.Reviews.AddRange(reviews);
        await _dbContext.SaveChangesAsync();
        return reviews.Count;
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

        var procedurePath = Path.Combine(
            AppContext.BaseDirectory,
            "SqlScripts",
            "sqlserver",
            "ClearTravelTestData.sql");
        var procedureSql = await File.ReadAllTextAsync(procedurePath);
        var connection = _dbContext.Database.GetDbConnection();
        var openedConnection = connection.State == System.Data.ConnectionState.Closed;
        if (openedConnection)
            await connection.OpenAsync();

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = procedureSql;
            await command.ExecuteNonQueryAsync();
        }
        finally
        {
            if (openedConnection)
                await connection.CloseAsync();
        }

        await _dbContext.Database.ExecuteSqlRawAsync("""
            CREATE OR ALTER VIEW dbo.TravelDatabaseOverview
            AS
            SELECT
                (SELECT COUNT(*) FROM dbo.[User]) AS UserCount,
                (SELECT COUNT(*) FROM dbo.City) AS CityCount,
                (SELECT COUNT(*) FROM dbo.Attraction) AS AttractionCount
            """);
    }

    public AdminDbRepos(ILogger<AdminDbRepos> logger, Encryptions encryptions, MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
}
