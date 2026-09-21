namespace Models;

public class Review : IReview
{
    public Guid ReviewId { get; set; }
    public Guid AttractionId { get; set; }
    public Guid UserId { get; set; }
    public string CommentText { get; set; }
    public byte Score { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Attraction Attraction { get; set; }
    public User User { get; set; }
}
