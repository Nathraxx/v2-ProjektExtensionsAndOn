using Models;

namespace Services;

public interface ICitiesService
{
    Task<List<City>> GetAllAsync();
    Task<List<City>> GetByCountryAsync(Guid countryId);
    Task<City?> GetByIdAsync(Guid cityId);
}