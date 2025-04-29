using System.ComponentModel.DataAnnotations;

namespace XrayUi.Infrastructure.Xray.Options.Server;

public class XrayDnsServerOptions : IValidatableObject
{
    public string Address { get; set; } = string.Empty;

    public int Port { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Address))
        {
            yield return new ValidationResult(
                "Dns server address must be specified",
                [nameof(Address)]);
        }

        if (Port < 1)
        {
            yield return new ValidationResult(
                "Dns server port must be positive",
                [nameof(Port)]);
        }
    }
}
