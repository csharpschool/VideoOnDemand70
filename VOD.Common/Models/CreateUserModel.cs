namespace VOD.Common.Models;

public class CreateUserModel
{
    [Required, EmailAddress, MinLength(6)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required, MinLength(6), DisplayName("Confirm Password"), Compare(nameof(Password), ErrorMessage = "Paswords don't match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public bool IsCustomer { get; set; } = false;
    public bool IsAdmin { get; set; } = false;
}
