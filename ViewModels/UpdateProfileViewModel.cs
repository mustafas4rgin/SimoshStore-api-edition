using System.ComponentModel.DataAnnotations;
using App.Data.Entities;

namespace SimoshStore;

public class UpdateProfileViewModel
{
    public UserEntity user { get; set; } = null!;
    [Required (ErrorMessage = "First name is required")][MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
    public string FirstName { get; set; } = default!;
    [Required (ErrorMessage = "Last name is required")][MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
    public string LastName { get; set; } = default!;
    [Required (ErrorMessage = "Phone is required")][MinLength(5, ErrorMessage = "Street must be at least 5 characters")]
    public string Phone { get; set; } = default!;
    [Required (ErrorMessage = "Email is required")][EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = default!;}
