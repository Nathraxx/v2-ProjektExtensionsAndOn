using Models;

namespace DbRepos;

public interface ICitiesDbRepos
{
    Task<List<City>> GetAllAsync();
    Task<List<City>> GetByCountryAsync(Guid countryId);
    Task<City?> GetByIdAsync(Guid cityId);
}