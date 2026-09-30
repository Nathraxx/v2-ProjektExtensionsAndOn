using Microsoft.Extensions.Logging;

using DbRepos;
using Models;

namespace Services;

public class AdminServiceDb : IAdminService
{
    private readonly IAdminDbRepos _repo = null;
    private readonly ILogger<AdminServiceDb> _logger = null;

    public Task SeedAsync(int nrItems) => _repo.SeedAsync(nrItems);
    public Task ClearTestDataAsync() => _repo.ClearTestDataAsync();
    public Task<DatabaseOverview> GetOverviewAsync() => _repo.GetOverviewAsync();

    #region constructors
    public AdminServiceDb(IAdminDbRepos repo)
    {
        _repo = repo;
    }
    public AdminServiceDb(IAdminDbRepos repo, ILogger<AdminServiceDb> logger) : this(repo)
    {
        _logger = logger;
    }
    #endregion
}

