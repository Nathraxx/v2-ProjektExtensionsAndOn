using Models;

namespace Services;

public interface IReviewsService
{
    Task<PagedResult<Review>> GetByAttractionAsync(Guid attractionId, int page, int pageSize);
    Task AddAsync(Review review);
}