namespace Models.DTO;

public class CitySummaryDto
{
    public Guid CityId { get; set; }
    public Guid CountryId { get; set; }
    public string CountryName { get; set; }
    public string Name { get; set; }
}

public class UserSummaryDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CategorySummaryDto
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
}

public class AttractionSummaryDto
{
    public Guid AttractionId { get; set; }
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public CitySummaryDto City { get; set; }
    public IReadOnlyList<CategorySummaryDto> Categories { get; set; } = Array.Empty<CategorySummaryDto>();
}

public class AttractionDetailsResponse
{
    public Guid AttractionId { get; set; }
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public IReadOnlyList<CategorySummaryDto> Categories { get; set; } = Array.Empty<CategorySummaryDto>();
    public PagedResult<Review> Reviews { get; set; } = new();
}

public class ReviewSummaryDto
{
    public Guid ReviewId { get; set; }
    public Guid AttractionId { get; set; }
    public Guid UserId { get; set; }
    public string CommentText { get; set; }
    public int Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public ReviewedAttractionSummaryDto Attraction { get; set; }
}

public class ReviewedAttractionSummaryDto
{
    public Guid AttractionId { get; set; }
    public string Name { get; set; }
    public CitySummaryDto City { get; set; }
}

public class AttractionReviewDto
{
    public Guid ReviewId { get; set; }
    public Guid AttractionId { get; set; }
    public Guid UserId { get; set; }
    public string CommentText { get; set; }
    public int Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public ReviewAuthorSummaryDto User { get; set; }
}

public class ReviewAuthorSummaryDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
}
