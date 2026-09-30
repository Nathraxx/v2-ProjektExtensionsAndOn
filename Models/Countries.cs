namespace Models;

public class Country : ICountry
{
    public virtual Guid CountryId { get; set; }
    public virtual string Name { get; set; }

    public ICollection<City> Cities { get; set; } = new List<City>();
}
