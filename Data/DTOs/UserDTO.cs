using System.ComponentModel.DataAnnotations;

namespace SimoshStore;

public class UserDTO
{
    [Required (ErrorMessage = "First name is required")][MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
    public string FirstName { get; set; } = default!;
    [Required (ErrorMessage = "Last name is required")][MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
    public string LastName { get; set; } = default!;
    [Required (ErrorMessage = "Street is required")][MinLength(5, ErrorMessage = "Street must be at least 5 characters")]
    public string Phone { get; set; } = default!;
    [Required (ErrorMessage = "Email is required")][EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = default!;
    [Required (ErrorMessage = "Password is required")][DataType(DataType.Password)][StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public byte[] PasswordHash { get; set; } = default!;
    public byte[] PasswordSalt { get; set; } = default!;
}