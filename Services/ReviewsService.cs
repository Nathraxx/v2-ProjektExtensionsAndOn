using Models;
using Models.DTO;
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
    public async Task<PagedResult<ReviewSummaryDto>> GetByUserAsync(Guid userId, int page, int pageSize)
    {
        var result = await _repo.GetByUserAsync(userId, page, pageSize);
        return new PagedResult<ReviewSummaryDto>
        {
            Items = result.Items.Select(review => new ReviewSummaryDto
            {
                ReviewId = review.ReviewId,
                AttractionId = review.AttractionId,
                UserId = review.UserId,
                CommentText = review.CommentText,
                Score = review.Score,
                CreatedAt = review.CreatedAt,
                Attraction = new ReviewedAttractionSummaryDto
                {
                    AttractionId = review.Attraction.AttractionId,
                    Name = review.Attraction.Name,
                    City = new CitySummaryDto
                    {
                        CityId = review.Attraction.City.CityId,
                        CountryId = review.Attraction.City.CountryId,
                        CountryName = review.Attraction.City.Country.Name,
                        Name = review.Attraction.City.Name
                    }
                }
            }).ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }
    public Task AddAsync(Review review) => _repo.AddAsync(review);
}
