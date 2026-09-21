using Models;

namespace DbRepos;

public interface IReviewsDbRepos
{
    Task<PagedResult<Review>> GetByAttractionAsync(Guid attractionId, int page, int pageSize);
    Task AddAsync(Review review);
}