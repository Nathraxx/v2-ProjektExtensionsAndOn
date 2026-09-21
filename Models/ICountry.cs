namespace Models;

public interface ICountry
{
    Guid CountryId { get; set; }
    string Name { get; set; }
}
