using Models;

namespace DbRepos;

public interface IAdminDbRepos
{
    Task SeedAsync(int nrItems);
    Task ClearTestDataAsync();
    Task<DatabaseOverview> GetOverviewAsync();
}