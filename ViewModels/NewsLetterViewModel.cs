using System.ComponentModel.DataAnnotations;

namespace SimoshStore;

public class NewsLetterViewModel
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "E-mail should be at least 6 characters.")]
    public string Email { get; set; } = null!;
}
