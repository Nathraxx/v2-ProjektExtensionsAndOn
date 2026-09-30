using Models;
using Models.DTO;
using DbRepos;

namespace Services;

public class AttractionsService : IAttractionsService
{
    private readonly IAttractionsDbRepos _repo;
    private readonly IReviewsDbRepos _reviewsRepo;

    public AttractionsService(IAttractionsDbRepos repo, IReviewsDbRepos reviewsRepo)
    {
        _repo = repo;
        _reviewsRepo = reviewsRepo;
    }

    public Task<List<Attraction>> GetAllAsync() => _repo.GetAllAsync();
    public async Task<PagedResult<AttractionSummaryDto>> SearchAsync(string? category, string? title, string? description, string? country, string? city, int page, int pageSize)
    {
        var result = await _repo.SearchAsync(category, title, description, country, city, page, pageSize);
        return new PagedResult<AttractionSummaryDto>
        {
            Items = result.Items.Select(attraction => new AttractionSummaryDto
            {
                AttractionId = attraction.AttractionId,
                CityId = attraction.CityId,
                Name = attraction.Name,
                Description = attraction.Description,
                Address = attraction.Address,
                CreatedAt = attraction.CreatedAt,
                City = new CitySummaryDto
                {
                    CityId = attraction.City.CityId,
                    CountryId = attraction.City.CountryId,
                    CountryName = attraction.City.Country.Name,
                    Name = attraction.City.Name
                },
                Categories = attraction.Categories.Select(category => new CategorySummaryDto
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name
                }).ToList()
            }).ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }
    public Task<PagedResult<Attraction>> GetWithoutReviewsAsync(int page, int pageSize)
        => _repo.GetWithoutReviewsAsync(page, pageSize);
    public Task<PagedResult<Attraction>> GetByCityAsync(Guid cityId, int page, int pageSize)
        => _repo.GetByCityAsync(cityId, page, pageSize);
    public async Task<AttractionDetailsResponse?> GetDetailsAsync(Guid attractionId, int page, int pageSize)
    {
        var attraction = await _repo.GetByIdAsync(attractionId);
        if (attraction == null)
            return null;

        var reviews = await _reviewsRepo.GetByAttractionAsync(attractionId, page, pageSize);
        return new AttractionDetailsResponse
        {
            AttractionId = attraction.AttractionId,
            CityId = attraction.CityId,
            Name = attraction.Name,
            Description = attraction.Description,
            Address = attraction.Address,
            CreatedAt = attraction.CreatedAt,
            Categories = attraction.Categories.Select(category => new CategorySummaryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            }).ToList(),
            Reviews = reviews
        };
    }
    public Task AddAsync(Attraction attraction) => _repo.AddAsync(attraction);
}
