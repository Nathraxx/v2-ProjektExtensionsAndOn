using Microsoft.EntityFrameworkCore;
using Models;
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
        => await _dbContext.Users
            .Include(u => u.Reviews)
            .AsNoTracking()
            .ToListAsync();

    public async Task<PagedResult<User>> GetPagedAsync(int page, int pageSize)
    {
        var query = _dbContext.Users
            .Include(u => u.Reviews)
                .ThenInclude(r => r.Attraction)
            .AsNoTracking();
        var totalCount = await query.CountAsync();
        var items = await query.OrderBy(u => u.Username)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<User>
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
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }
}
