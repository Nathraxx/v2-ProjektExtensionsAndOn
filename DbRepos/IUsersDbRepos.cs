using Models;
using Models.DTO;

namespace DbRepos;

public interface IUsersDbRepos
{
    Task<List<User>> GetAllAsync();
    Task<PagedResult<UserSummaryDto>> GetPagedAsync(int page, int pageSize);
    Task<User?> GetByIdAsync(Guid userId);
    Task AddAsync(User user);
}