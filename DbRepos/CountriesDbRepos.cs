using Microsoft.EntityFrameworkCore;
using Models;
using DbContext;

namespace DbRepos;

public class CountriesDbRepos
{
    private readonly MainDbContext _dbContext;

    public CountriesDbRepos(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Country>> GetAllAsync()
        => await _dbContext.Countries
            .Include(c => c.Cities)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Country?> GetByIdAsync(Guid countryId)
        => await _dbContext.Countries
            .Include(c => c.Cities)
            .FirstOrDefaultAsync(c => c.CountryId == countryId);
}
