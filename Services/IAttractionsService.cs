using Models;
using Models.DTO;

namespace Services;

public interface IAttractionsService
{
    Task<List<Attraction>> GetAllAsync();
    Task<PagedResult<AttractionSummaryDto>> SearchAsync(string? category, string? title, string? description, string? country, string? city, int page, int pageSize);
    Task<PagedResult<Attraction>> GetWithoutReviewsAsync(int page, int pageSize);
    Task<PagedResult<Attraction>> GetByCityAsync(Guid cityId, int page, int pageSize);
    Task<AttractionDetailsResponse?> GetDetailsAsync(Guid attractionId, int page, int pageSize);
    Task AddAsync(Attraction attraction);
}