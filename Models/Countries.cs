namespace Models;

public class Country : ICountry
{
    public Guid CountryId { get; set; }
    public string Name { get; set; }

    public ICollection<City> Cities { get; set; } = new List<City>();
}
