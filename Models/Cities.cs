namespace Models;

public class City : ICity
{
    public virtual Guid CityId { get; set; }
    public virtual Guid CountryId { get; set; }
    public virtual string Name { get; set; }

    public Country Country { get; set; }
    public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
}
