using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using DbRepos;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TravelController : ControllerBase
{
    private readonly ICitiesService _citiesService;
    private readonly IAttractionsService _attractionsService;
    private readonly IReviewsService _reviewsService;
    private readonly IUsersService _usersService;
    private readonly IAdminDbRepos _adminDbRepos;

    public TravelController(
        ICitiesService citiesService,
        IAttractionsService attractionsService,
        IReviewsService reviewsService,
        IUsersService usersService,
        IAdminDbRepos adminDbRepos)
    {
        _citiesService = citiesService;
        _attractionsService = attractionsService;
        _reviewsService = reviewsService;
        _usersService = usersService;
        _adminDbRepos = adminDbRepos;
    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed()
    {
        await _adminDbRepos.SeedAsync(0);
        return Ok(new
        {
            message = "Seeded assignment test data.",
            countries = 4,
            cities = 100,
            attractions = 1000,
            users = 50,
            reviewsPerAttraction = "0-20"
        });
    }

    [HttpGet("cities")]
    public async Task<IActionResult> GetCities()
    {
        var cities = await _citiesService.GetAllAsync();
        return Ok(cities);
    }

    [HttpGet("cities/{cityId:guid}/attractions")]
    public async Task<IActionResult> GetAttractionsByCity(
        Guid cityId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var attractions = await _attractionsService.GetByCityAsync(cityId, page, pageSize);
        return Ok(attractions);
    }

    [HttpGet("attractions")]
    public async Task<IActionResult> GetAttractions(
        [FromQuery] string? category = null,
        [FromQuery] string? title = null,
        [FromQuery] string? description = null,
        [FromQuery] string? country = null,
        [FromQuery] string? city = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var attractions = await _attractionsService.SearchAsync(category, title, description, country, city, page, pageSize);
        return Ok(attractions);
    }

    [HttpGet("attractions/without-reviews")]
    public async Task<IActionResult> GetAttractionsWithoutReviews(int page = 1, int pageSize = 20)
        => Ok(await _attractionsService.GetWithoutReviewsAsync(Math.Max(1, page), Math.Clamp(pageSize, 1, 100)));

    [HttpGet("attractions/{attractionId:guid}")]
    public async Task<IActionResult> GetAttraction(Guid attractionId)
    {
        var attraction = await _attractionsService.GetByIdAsync(attractionId);
        if (attraction == null)
            return NotFound();

        return Ok(attraction);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(int page = 1, int pageSize = 20)
        => Ok(await _usersService.GetPagedAsync(Math.Max(1, page), Math.Clamp(pageSize, 1, 100)));

    [HttpGet("attractions/{attractionId:guid}/reviews")]
    public async Task<IActionResult> GetReviewsForAttraction(
        Guid attractionId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var reviews = await _reviewsService.GetByAttractionAsync(attractionId, page, pageSize);
        return Ok(reviews);
    }

    [HttpPost("attractions/{attractionId:guid}/reviews")]
    public async Task<IActionResult> AddReview(Guid attractionId, [FromBody] Review review)
    {
        if (review == null)
            return BadRequest();

        review.AttractionId = attractionId;
        review.CreatedAt = DateTime.UtcNow;

        await _reviewsService.AddAsync(review);
        return Ok(review);
    }

    [HttpPost("users")]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        if (user == null)
            return BadRequest();

        user.CreatedAt = DateTime.UtcNow;
        await _usersService.AddAsync(user);
        return Ok(user);
    }
}
