using XrayUi.Application.Models;

namespace XrayUi.Application.Abstractions.Xray;

public interface IXrayClientConfigFormatter
{
    string Format(XrayClientConfig config);
}
