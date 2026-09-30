namespace Models;

public interface IReview
{
    Guid ReviewId { get; set; }
    Guid AttractionId { get; set; }
    Guid UserId { get; set; }
    string CommentText { get; set; }
    int Score { get; set; }
    DateTime CreatedAt { get; set; }
}
