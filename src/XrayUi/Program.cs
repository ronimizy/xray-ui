#pragma warning disable CA1506
using Blazorise;
using Blazorise.Bootstrap5;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;
using Phazor.Components.Extensions;
using XrayUi.Application;
using XrayUi.Authorization;
using XrayUi.Frontend;
using XrayUi.Infrastructure.Persistence;
using XrayUi.Infrastructure.Xray;
using XrayUi.Options;
using XrayUi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("xray-ui-config.json", optional: true);

builder.Services
    .AddPersistence(builder.Configuration)
    .AddApplication()
    .AddInfrastructureXray();

builder.Services.AddScoped<XrayAuthorizationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<XrayAuthorizationStateProvider>());
builder.Services.AddAuthentication().AddCookie(x => x.LoginPath = "/login");
builder.Services.AddAuthorization();

builder.Services
    .AddOptions<XrayAuthorizationOptions>()
    .BindConfiguration("Authorization")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddMudServices()
    .AddMudMarkdownServices()
    .AddMudMarkdownClipboardService<CustomClipboardService>()
    .AddBlazorise()
    .AddBootstrap5Providers()
    .AddPhazorComponents();

builder.WebHost.UseStaticWebAssets();
builder.Services.AddHttpContextAccessor();

WebApplication app = builder.Build();

app.UseStaticFiles();
app.UseAntiforgery();

app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
