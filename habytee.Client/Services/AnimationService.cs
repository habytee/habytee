using Microsoft.JSInterop;

namespace habytee.Client.Services;

public class AnimationService
{
    private readonly IJSRuntime js;

    public AnimationService(IJSRuntime js)
    {
        this.js = js;
    }

    public async Task<bool> AnimateCoin()
    {
        return await js.InvokeAsync<bool>("animateCoin");
    }
}
