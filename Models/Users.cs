namespace Models;

public class User : IUser
{
    public virtual Guid UserId { get; set; }
    public virtual string Username { get; set; }
    public virtual string Email { get; set; }
    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
