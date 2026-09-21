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
        => await _dbContext.Cities
            .Include(c => c.Country)
            .Include(c => c.Attractions)
            .AsNoTracking()
            .ToListAsync();

    public async Task<List<City>> GetByCountryAsync(Guid countryId)
        => await _dbContext.Cities
            .Where(c => c.CountryId == countryId)
            .Include(c => c.Attractions)
            .AsNoTracking()
            .ToListAsync();

    public async Task<City?> GetByIdAsync(Guid cityId)
        => await _dbContext.Cities
            .Include(c => c.Country)
            .Include(c => c.Attractions)
            .FirstOrDefaultAsync(c => c.CityId == cityId);
}
