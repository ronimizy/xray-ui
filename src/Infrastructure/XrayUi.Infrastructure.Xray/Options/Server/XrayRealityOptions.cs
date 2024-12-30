using System.ComponentModel.DataAnnotations;

namespace XrayUi.Infrastructure.Xray.Options.Server;

public class XrayRealityOptions : IValidatableObject
{
    public string Destination { get; set; } = string.Empty;

    public string[] ServerNames { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Destination))
            yield return new ValidationResult("Reality destination must be specified");

        if (ServerNames is [])
            yield return new ValidationResult("Reality server names must be specified");
    }
}
