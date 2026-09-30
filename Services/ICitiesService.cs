using Models;

namespace Services;

public interface ICitiesService
{
    Task<List<City>> GetAllAsync();
    Task<PagedResult<Models.DTO.CitySummaryDto>> GetPagedAsync(int page, int pageSize);
    Task<List<City>> GetByCountryAsync(Guid countryId);
    Task<City?> GetByIdAsync(Guid cityId);
}