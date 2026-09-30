using Microsoft.EntityFrameworkCore;
using Models;
using DbContext;

namespace DbRepos;

public class ReviewsDbRepos : IReviewsDbRepos
{
    private readonly MainDbContext _dbContext;

    public ReviewsDbRepos(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<Review>> GetByAttractionAsync(Guid attractionId, int page, int pageSize)
    {
        var query = _dbContext.Reviews
            .Where(r => r.AttractionId == attractionId)
            .Include(r => r.User)
            .AsNoTracking();
        var totalCount = await query.CountAsync();
        var items = await query.OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Review>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PagedResult<Review>> GetByUserAsync(Guid userId, int page, int pageSize)
    {
        var query = _dbContext.Reviews
            .Where(review => review.UserId == userId)
            .Include(review => review.Attraction)
                .ThenInclude(attraction => attraction.City)
                    .ThenInclude(city => city.Country)
            .AsNoTracking();
        var totalCount = await query.CountAsync();
        var items = await query.OrderByDescending(review => review.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Review>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(Review review)
    {
        _dbContext.Reviews.Add(new DbModels.ReviewDbM
        {
            ReviewId = review.ReviewId,
            AttractionId = review.AttractionId,
            UserId = review.UserId,
            CommentText = review.CommentText,
            Score = review.Score,
            CreatedAt = review.CreatedAt
        });
        await _dbContext.SaveChangesAsync();
    }
}
