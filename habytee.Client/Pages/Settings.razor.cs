using Microsoft.AspNetCore.Components;
using habytee.Client.ViewModels;
using habytee.Client.Services;
using Radzen.Blazor;

namespace habytee.Client.Pages;

public partial class Settings
{
    private RadzenAppearanceToggle? appearanceToggle;
    private SettingsViewModel settingsViewModel = null!;

    [Inject]
    private ApiService ApiService { get; set; } = default!;

    [Inject]
    private BrowserDetectThemeService BrowserDetectThemeService { get; set; } = default!;

    [Inject]
    private MainViewModel mainViewModel { get; set; } = default!;

    protected override void OnInitialized()
    {
        settingsViewModel = new SettingsViewModel(ApiService, BrowserDetectThemeService, mainViewModel);
    }
}
