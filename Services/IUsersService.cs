using Models;

namespace Services;

public interface IUsersService
{
    Task<List<User>> GetAllAsync();
    Task<PagedResult<User>> GetPagedAsync(int page, int pageSize);
    Task<User?> GetByIdAsync(Guid userId);
    Task AddAsync(User user);
}