using Microsoft.EntityFrameworkCore;
using Models;
using DbContext;

namespace DbRepos;

public class CitiesDbRepos : ICitiesDbRepos
{
    private readonly MainDbContext _dbContext;

    public CitiesDbRepos(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<City>> GetAllAsync()
    {
        var cities = await _dbContext.Cities
            .Include(c => c.Country)
            .Include(c => c.Attractions)
                .ThenInclude(a => a.Categories)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();

        return cities.Select(city => new City
        {
            CityId = city.CityId,
            CountryId = city.CountryId,
            Name = city.Name,
            Country = city.Country == null ? null : new Country
            {
                CountryId = city.Country.CountryId,
                Name = city.Country.Name
            },
            Attractions = city.Attractions.Select(attraction => new Attraction
            {
                AttractionId = attraction.AttractionId,
                CityId = attraction.CityId,
                Name = attraction.Name,
                Description = attraction.Description,
                Address = attraction.Address,
                CreatedAt = attraction.CreatedAt,
                Categories = attraction.Categories.Select(category => new Category
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name
                }).ToList()
            }).ToList()
        }).ToList();
    }

    public async Task<PagedResult<Models.DTO.CitySummaryDto>> GetPagedAsync(int page, int pageSize)
    {
        var query = _dbContext.Cities
            .Include(city => city.Country)
            .AsNoTracking();
        var totalCount = await query.CountAsync();
        var items = await query.OrderBy(city => city.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(city => new Models.DTO.CitySummaryDto
            {
                CityId = city.CityId,
                CountryId = city.CountryId,
                CountryName = city.Country.Name,
                Name = city.Name
            })
            .ToListAsync();

        return new PagedResult<Models.DTO.CitySummaryDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<List<City>> GetByCountryAsync(Guid countryId)
    {
        var cities = await _dbContext.Cities
            .Where(c => c.CountryId == countryId)
            .Include(c => c.Attractions)
            .AsNoTracking()
            .ToListAsync();
        return cities.Cast<City>().ToList();
    }

    public async Task<City?> GetByIdAsync(Guid cityId)
        => await _dbContext.Cities
            .Include(c => c.Country)
            .Include(c => c.Attractions)
            .FirstOrDefaultAsync(c => c.CityId == cityId);
}
