using Models;
using DbRepos;

namespace Services;

public class ReviewsService : IReviewsService
{
    private readonly IReviewsDbRepos _repo;

    public ReviewsService(IReviewsDbRepos repo)
    {
        _repo = repo;
    }

    public Task<PagedResult<Review>> GetByAttractionAsync(Guid attractionId, int page, int pageSize)
        => _repo.GetByAttractionAsync(attractionId, page, pageSize);
    public Task AddAsync(Review review) => _repo.AddAsync(review);
}
