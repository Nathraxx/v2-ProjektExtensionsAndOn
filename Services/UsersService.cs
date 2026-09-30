using Models;
using Models.DTO;
using DbRepos;

namespace Services;

public class UsersService : IUsersService
{
    private readonly IUsersDbRepos _repo;

    public UsersService(IUsersDbRepos repo)
    {
        _repo = repo;
    }

    public Task<List<User>> GetAllAsync() => _repo.GetAllAsync();
    public Task<PagedResult<UserSummaryDto>> GetPagedAsync(int page, int pageSize) => _repo.GetPagedAsync(page, pageSize);
    public Task<User?> GetByIdAsync(Guid userId) => _repo.GetByIdAsync(userId);
    public Task AddAsync(User user) => _repo.AddAsync(user);
}
