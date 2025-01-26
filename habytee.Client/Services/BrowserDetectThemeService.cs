using Microsoft.JSInterop;

namespace habytee.Client.Services;

public class BrowserDetectThemeService
{
    private readonly IJSRuntime js;

    public BrowserDetectThemeService(IJSRuntime js)
    {
        this.js = js;
    }

    public async Task<bool> IsDarkMode()
    {
        return await js.InvokeAsync<bool>("IsDarkMode");
    }
}
