namespace Models;

public interface IAttraction
{
    Guid AttractionId { get; set; }
    Guid CityId { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    string Address { get; set; }
    DateTime CreatedAt { get; set; }
}
