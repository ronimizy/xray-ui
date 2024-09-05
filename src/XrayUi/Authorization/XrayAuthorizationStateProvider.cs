using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using XrayUi.Options;

namespace XrayUi.Authorization;

public class XrayAuthorizationStateProvider : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal UnauthorizedPrincipal = new();
    private static readonly ClaimsPrincipal AuthorizedPrincipal = new(new ClaimsIdentity("simple"));

    private ClaimsPrincipal _principal;

    public XrayAuthorizationStateProvider(IOptions<XrayAuthorizationOptions> options)
    {
        _principal = string.IsNullOrEmpty(options.Value.Password)
            ? AuthorizedPrincipal
            : UnauthorizedPrincipal;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var state = new AuthenticationState(_principal);
        return Task.FromResult(state);
    }

    public void OnLoggedIn()
    {
        _principal = AuthorizedPrincipal;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
