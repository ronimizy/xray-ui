using System.ComponentModel.DataAnnotations;

namespace XrayUi.Options;

public class XrayAuthorizationOptions : IValidatableObject
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Username))
            yield return new ValidationResult("Authorization username should not be empty");

        if (string.IsNullOrEmpty(Password))
            yield return new ValidationResult("Authorization password should not be empty");
    }
}
