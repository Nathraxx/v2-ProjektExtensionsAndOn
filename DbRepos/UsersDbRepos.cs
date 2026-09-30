using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;
using DbContext;

namespace DbRepos;

public class UsersDbRepos : IUsersDbRepos
{
    private readonly MainDbContext _dbContext;

    public UsersDbRepos(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> GetAllAsync()
    {
        var users = await _dbContext.Users
            .Include(u => u.Reviews)
            .AsNoTracking()
            .ToListAsync();
        return users.Cast<User>().ToList();
    }

    public async Task<PagedResult<UserSummaryDto>> GetPagedAsync(int page, int pageSize)
    {
        var query = _dbContext.Users.AsNoTracking();
        var totalCount = await query.CountAsync();
        var items = await query.OrderBy(u => u.Username)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(user => new UserSummaryDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Reviews = user.Reviews
                    .OrderByDescending(review => review.CreatedAt)
                    .Select(review => new UserReviewSummaryDto
                    {
                        ReviewId = review.ReviewId,
                        AttractionId = review.AttractionId,
                        AttractionName = review.Attraction.Name,
                        CommentText = review.CommentText,
                        Score = review.Score,
                        CreatedAt = review.CreatedAt
                    })
                    .ToList()
            })
            .ToListAsync();

        return new PagedResult<UserSummaryDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<User?> GetByIdAsync(Guid userId)
        => await _dbContext.Users
            .Include(u => u.Reviews)
            .FirstOrDefaultAsync(u => u.UserId == userId);

    public async Task AddAsync(User user)
    {
        _dbContext.Users.Add(new DbModels.UserDbM
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        });
        await _dbContext.SaveChangesAsync();
    }
}
