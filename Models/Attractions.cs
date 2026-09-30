namespace Models;

public class Attraction : IAttraction
{
    public virtual Guid AttractionId { get; set; }
    public virtual Guid CityId { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual string Address { get; set; }
    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public City City { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
