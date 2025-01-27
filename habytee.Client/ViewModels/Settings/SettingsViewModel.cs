using habytee.Client.Services;
using habytee.Interconnection.Models;
using Habytee.Interconnection.Dto;

namespace habytee.Client.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly ApiService apiService;
    private readonly BrowserDetectThemeService browserDetectThemeService;
    private readonly MainViewModel mainViewModel;
    private User? user;

    public SettingsViewModel(ApiService apiService, BrowserDetectThemeService browserDetectThemeService, MainViewModel mainViewModel)
    {
        this.apiService = apiService;
        this.browserDetectThemeService = browserDetectThemeService;
        this.mainViewModel = mainViewModel;
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        user = await apiService.GetUserAsync();
    }

    public async Task OnThemeChanged(bool isLightTheme)
    {
        if (user != null)
        {
            user.LightTheme = isLightTheme;
            await apiService.UpdateUserAsync(UpdateUserDto.CreateUpdateUserDto(user));
        }
    }
}
