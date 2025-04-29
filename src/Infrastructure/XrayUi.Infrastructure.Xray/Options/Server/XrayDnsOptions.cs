using System.ComponentModel.DataAnnotations;

namespace XrayUi.Infrastructure.Xray.Options.Server;

public class XrayDnsOptions : IValidatableObject
{
    public XrayDnsServerOptions[] Servers { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return Servers.SelectMany(server => server.Validate(validationContext));
    }
}
