using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace habytee.Client.Pages;

public partial class Settings
{
    private RadzenAppearanceToggle? appearanceToggle;

    [Inject]
    private ThemeService ThemeService { get; set; } = default!;

    protected override void OnInitialized()
    {
        ThemeService.SetTheme("Material");
    }
}
