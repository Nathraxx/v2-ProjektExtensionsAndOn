namespace Models;

public interface IUser
{
    Guid UserId { get; set; }
    string Username { get; set; }
    string Email { get; set; }
    DateTime CreatedAt { get; set; }
}
