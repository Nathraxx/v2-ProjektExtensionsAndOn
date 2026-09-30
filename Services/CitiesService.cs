using Models;
using DbRepos;

namespace Services;

public class CitiesService : ICitiesService
{
    private readonly ICitiesDbRepos _repo;

    public CitiesService(ICitiesDbRepos repo)
    {
        _repo = repo;
    }

    public Task<List<City>> GetAllAsync() => _repo.GetAllAsync();
    public Task<PagedResult<Models.DTO.CitySummaryDto>> GetPagedAsync(int page, int pageSize)
        => _repo.GetPagedAsync(page, pageSize);
    public Task<List<City>> GetByCountryAsync(Guid countryId) => _repo.GetByCountryAsync(countryId);
    public Task<City?> GetByIdAsync(Guid cityId) => _repo.GetByIdAsync(cityId);
}
