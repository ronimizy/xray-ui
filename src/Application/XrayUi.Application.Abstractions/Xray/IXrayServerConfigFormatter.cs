using XrayUi.Application.Models;

namespace XrayUi.Application.Abstractions.Xray;

public interface IXrayServerConfigFormatter
{
    string Format(XrayServerConfig config);
}
