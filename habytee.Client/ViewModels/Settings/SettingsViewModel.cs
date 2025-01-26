using habytee.Client.Services;

namespace habytee.Client.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly IApiService apiService;
    public event EventHandler? ThemeChanged;

    public SettingsViewModel(IApiService apiService)
    {
        this.apiService = apiService;
    }

    public void OnThemeChanged()
    {
        ThemeChanged?.Invoke(this, EventArgs.Empty);
    }
}
