using Models;

namespace Services;

public interface IAdminService
{
    public Task SeedAsync(int nrItems);
    public Task ClearTestDataAsync();
    public Task<DatabaseOverview> GetOverviewAsync();
}
