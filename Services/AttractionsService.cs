using Models;
using DbRepos;

namespace Services;

public class AttractionsService : IAttractionsService
{
    private readonly IAttractionsDbRepos _repo;

    public AttractionsService(IAttractionsDbRepos repo)
    {
        _repo = repo;
    }

    public Task<List<Attraction>> GetAllAsync() => _repo.GetAllAsync();
    public Task<PagedResult<Attraction>> SearchAsync(string? category, string? title, string? description, string? country, string? city, int page, int pageSize)
        => _repo.SearchAsync(category, title, description, country, city, page, pageSize);
    public Task<PagedResult<Attraction>> GetWithoutReviewsAsync(int page, int pageSize)
        => _repo.GetWithoutReviewsAsync(page, pageSize);
    public Task<PagedResult<Attraction>> GetByCityAsync(Guid cityId, int page, int pageSize)
        => _repo.GetByCityAsync(cityId, page, pageSize);
    public Task<Attraction?> GetByIdAsync(Guid attractionId) => _repo.GetByIdAsync(attractionId);
    public Task AddAsync(Attraction attraction) => _repo.AddAsync(attraction);
}
