using System.ComponentModel.DataAnnotations;

namespace JWTLogin.ViewModel;

public class UserInfoViewModel
{
    [Required]
    [MinLength(3)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password))]
    public string PasswordConfirm { get; set; } = string.Empty;
}
