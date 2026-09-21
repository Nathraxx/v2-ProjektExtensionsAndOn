namespace Models;

public class Attraction : IAttraction
{
    public Guid AttractionId { get; set; }
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public City City { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
