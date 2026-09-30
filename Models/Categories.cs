namespace Models;

public class Category : ICategory
{
    public virtual Guid CategoryId { get; set; }
    public virtual string Name { get; set; }

    public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
}
