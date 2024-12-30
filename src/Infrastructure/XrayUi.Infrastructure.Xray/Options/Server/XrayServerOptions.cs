using System.ComponentModel.DataAnnotations;

namespace XrayUi.Infrastructure.Xray.Options.Server;

public class XrayServerOptions : IValidatableObject
{
    public string LogLevel { get; set; } = string.Empty;

    public int InboundPort { get; set; }

    public string PrivateKey { get; set; } = string.Empty;

    public XrayRealityOptions Reality { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (InboundPort is 0)
            yield return new ValidationResult("Inbound port must be specified");

        if (string.IsNullOrEmpty(PrivateKey))
            yield return new ValidationResult("Private key must be specified");

        foreach (ValidationResult result in Reality.Validate(validationContext))
        {
            yield return result;
        }
    }
}
