using System.ComponentModel.DataAnnotations;

namespace SimoshStore;

public class EditAddressViewModel
{
    [Required (ErrorMessage = "First name is required")][MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
    public string FirstName { get; set; } = null!;
    [Required (ErrorMessage = "Last name is required")][MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
    public string LastName { get; set; } = null!;
    [Required (ErrorMessage = "Street is required")][MinLength(5, ErrorMessage = "Street must be at least 5 characters")]
    public string Street { get; set; } = null!;
    [Required (ErrorMessage = "Post code is required")][MinLength(5, ErrorMessage = "Post code must be at least 5 characters")]
    public string PostCode { get; set; } = null!;
    [Required (ErrorMessage = "Phone is required")][Phone(ErrorMessage = "Phone is not valid")]
    public string Phone { get; set; } = null!;
    [Required (ErrorMessage = "Email is required")][EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = null!;
}
