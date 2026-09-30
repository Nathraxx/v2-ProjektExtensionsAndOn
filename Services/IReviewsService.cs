using Models;
using Models.DTO;

namespace Services;

public interface IReviewsService
{
    Task<PagedResult<AttractionReviewDto>> GetByAttractionAsync(Guid attractionId, int page, int pageSize);
    Task<PagedResult<ReviewSummaryDto>> GetByUserAsync(Guid userId, int page, int pageSize);
    Task AddAsync(Review review);
}