using System.ComponentModel.DataAnnotations;

namespace Models.DTO;

public class CreateReviewRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string CommentText { get; set; }

    [Range(1, 5)]
    public int Score { get; set; }
}