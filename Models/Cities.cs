namespace Models;

public class City : ICity
{
    public Guid CityId { get; set; }
    public Guid CountryId { get; set; }
    public string Name { get; set; }

    public Country Country { get; set; }
    public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
}
