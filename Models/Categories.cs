namespace Models;

public class Category : ICategory
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; }

    public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
}
