using habytee.Client.Services;
using habytee.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace habytee.Client.Layout;

public partial class MainLayout
{
    [Inject]
    private ApiService apiService { get; set; } = default!;

    [Inject]
    private AnimationService animationService { get; set; } = default!;

    [Inject]
    private BrowserDetectThemeService browserDetectThemeService { get; set; } = default!;

    [Inject]
    private ThemeService themeService { get; set; } = default!;

    [Inject]
    private MainViewModel mainViewModel { get; set; } = null!;
    
    protected override async Task OnInitializedAsync()
    {
        await mainViewModel.InitializeAsync();
        mainViewModel.PropertyChanged += (_, _) => StateHasChanged();
        mainViewModel.ThemeChanged += (s, e) => InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        if (mainViewModel != null)
        {
            mainViewModel.ThemeChanged -= (s, e) => InvokeAsync(StateHasChanged);
        }
    }
}
