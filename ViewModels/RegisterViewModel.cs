using System.ComponentModel.DataAnnotations;

namespace SimoshStore
{
    public class RegisterViewModel
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Phone]
    public string Phone { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}

}

