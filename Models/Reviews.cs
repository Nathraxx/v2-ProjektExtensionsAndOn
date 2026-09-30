namespace Models;

public class Review : IReview
{
    public virtual Guid ReviewId { get; set; }
    public virtual Guid AttractionId { get; set; }
    public virtual Guid UserId { get; set; }
    public virtual string CommentText { get; set; }
    public virtual int Score { get; set; }
    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Attraction Attraction { get; set; }
    public User User { get; set; }
}
