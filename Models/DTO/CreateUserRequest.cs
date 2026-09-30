using System.ComponentModel.DataAnnotations;

namespace Models.DTO;

public class CreateUserRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}