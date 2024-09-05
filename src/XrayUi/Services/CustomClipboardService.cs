using Microsoft.JSInterop;
using MudBlazor;

namespace XrayUi.Services;

public class CustomClipboardService : IMudMarkdownClipboardService
{
    private readonly IJSRuntime _js;

    public CustomClipboardService(IJSRuntime js)
    {
        _js = js;
    }

    public async ValueTask CopyToClipboardAsync(string text)
    {
        await _js.InvokeVoidAsync("customCopyToClipboard", text);
    }
}
