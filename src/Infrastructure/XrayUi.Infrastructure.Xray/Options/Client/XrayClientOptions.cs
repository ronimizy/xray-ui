using System.ComponentModel.DataAnnotations;

namespace XrayUi.Infrastructure.Xray.Options.Client;

public class XrayClientOptions : IValidatableObject
{
    public string LogLevel { get; set; } = string.Empty;

    public string ServerAddress { get; set; } = string.Empty;

    public int ServerPort { get; set; }

    public string PublicKey { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(ServerAddress))
            yield return new ValidationResult("Server address must be specified");

        if (ServerPort is 0)
            yield return new ValidationResult("Server port must be specified");

        if (string.IsNullOrEmpty(PublicKey))
            yield return new ValidationResult("Public key must be specified");
    }
}
