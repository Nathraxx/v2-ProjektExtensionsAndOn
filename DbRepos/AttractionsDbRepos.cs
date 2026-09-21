using Microsoft.EntityFrameworkCore;
using Models;
using DbContext;

namespace DbRepos;

public class AttractionsDbRepos : IAttractionsDbRepos
{
    private readonly MainDbContext _dbContext;

    public AttractionsDbRepos(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Attraction>> GetAllAsync()
        => await _dbContext.Attractions
            .Include(a => a.City)
            .Include(a => a.Categories)
            .AsNoTracking()
            .ToListAsync();

    public async Task<PagedResult<Attraction>> SearchAsync(
        string? category, string? title, string? description,
        string? country, string? city, int page, int pageSize)
    {
        var query = _dbContext.Attractions
            .Include(a => a.City)
                .ThenInclude(c => c.Country)
            .Include(a => a.Categories)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(a => a.Categories.Any(c => c.Name.Contains(category)));
        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(a => a.Name.Contains(title));
        if (!string.IsNullOrWhiteSpace(description))
            query = query.Where(a => a.Description.Contains(description));
        if (!string.IsNullOrWhiteSpace(country))
            query = query.Where(a => a.City.Country.Name.Contains(country));
        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(a => a.City.Name.Contains(city));

        var totalCount = await query.CountAsync();
        var items = await query.OrderBy(a => a.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Attraction>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PagedResult<Attraction>> GetWithoutReviewsAsync(int page, int pageSize)
    {
        var query = _dbContext.Attractions
            .Where(a => !a.Reviews.Any())
            .Include(a => a.City)
            .Include(a => a.Categories)
            .AsNoTracking();
        var totalCount = await query.CountAsync();
        var items = await query.OrderBy(a => a.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Attraction>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PagedResult<Attraction>> GetByCityAsync(Guid cityId, int page, int pageSize)
    {
        var query = _dbContext.Attractions
            .Where(a => a.CityId == cityId)
            .Include(a => a.Categories)
            .Include(a => a.City)
            .AsNoTracking();
        var totalCount = await query.CountAsync();
        var items = await query.OrderBy(a => a.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Attraction>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Attraction?> GetByIdAsync(Guid attractionId)
        => await _dbContext.Attractions
            .Include(a => a.City)
            .Include(a => a.Categories)
            .Include(a => a.Reviews)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(a => a.AttractionId == attractionId);

    public async Task AddAsync(Attraction attraction)
    {
        _dbContext.Attractions.Add(attraction);
        await _dbContext.SaveChangesAsync();
    }
}
