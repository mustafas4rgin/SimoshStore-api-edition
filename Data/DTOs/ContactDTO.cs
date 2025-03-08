using System.ComponentModel.DataAnnotations;

namespace SimoshStore;

public class ContactDTO
{
    [Required]
    [StringLength(30, MinimumLength = 10, ErrorMessage = "Name must be between 10 and 30 characters.")]
    public string Name { get; set; } = null!;
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [StringLength(50, MinimumLength = 10, ErrorMessage = "Email must be between 10 and 50 characters.")]
    public string Email { get; set; } = null!;
    [Required]
    [StringLength(100, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 100 characters.")]
    public string Message { get; set; } = null!;

}
