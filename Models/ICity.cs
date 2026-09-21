namespace Models;

public interface ICity
{
    Guid CityId { get; set; }
    Guid CountryId { get; set; }
    string Name { get; set; }
}
