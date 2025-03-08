using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
{
    [Required]
    public string Token { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Yeni şifre ile şifre doğrulama aynı olmalıdır.")]
    public string ConfirmPassword { get; set; }
}
