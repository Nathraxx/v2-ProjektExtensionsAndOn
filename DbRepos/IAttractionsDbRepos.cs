using Models;

namespace DbRepos;

public interface IAttractionsDbRepos
{
    Task<List<Attraction>> GetAllAsync();
    Task<PagedResult<Attraction>> SearchAsync(string? category, string? title, string? description, string? country, string? city, int page, int pageSize);
    Task<PagedResult<Attraction>> GetWithoutReviewsAsync(int page, int pageSize);
    Task<PagedResult<Attraction>> GetByCityAsync(Guid cityId, int page, int pageSize);
    Task<Attraction?> GetByIdAsync(Guid attractionId);
    Task AddAsync(Attraction attraction);
}