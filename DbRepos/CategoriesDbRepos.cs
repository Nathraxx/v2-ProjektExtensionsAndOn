using Microsoft.EntityFrameworkCore;
using Models;
using DbContext;

namespace DbRepos;

public class CategoriesDbRepos
{
    private readonly MainDbContext _dbContext;

    public CategoriesDbRepos(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        var categories = await _dbContext.Categories
            .AsNoTracking()
            .ToListAsync();
        return categories.Cast<Category>().ToList();
    }

    public async Task<Category?> GetByIdAsync(Guid categoryId)
        => await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
}
