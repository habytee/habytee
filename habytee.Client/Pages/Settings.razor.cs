using Microsoft.AspNetCore.Components;
using Radzen;
using habytee.Client.ViewModels;
using habytee.Client.Services;
using Radzen.Blazor;

namespace habytee.Client.Pages;

public partial class Settings
{
    private RadzenAppearanceToggle? appearanceToggle;
    private SettingsViewModel? settingsViewModel;

    [Inject]
    private ThemeService ThemeService { get; set; } = default!;

    [Inject]
    private ApiService ApiService { get; set; } = default!;

    protected override void OnInitialized()
    {
        settingsViewModel = new SettingsViewModel(ApiService);
        settingsViewModel.ThemeChanged += (s, e) => InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        if (settingsViewModel != null)
        {
            settingsViewModel.ThemeChanged -= (s, e) => InvokeAsync(StateHasChanged);
        }
    }
}
