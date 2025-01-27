using habytee.Client.Services;
using habytee.Interconnection.Models;
using Habytee.Interconnection.Dto;
using Radzen;

namespace habytee.Client.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly ApiService apiService;
    private readonly AnimationService animationService;
    private readonly BrowserDetectThemeService browserDetectThemeService;
    private readonly ThemeService themeService;

    public bool? DefaultSidebarExpanded = null;

    private bool sidebarExpanded = true;
    public bool SidebarExpanded 
    { 
        get => sidebarExpanded;
        set
        {
            SetProperty(ref sidebarExpanded, value);
            if(DefaultSidebarExpanded == null)
            {
                DefaultSidebarExpanded = value;
            }
        }
    }

    private string email = "Profile";
    public string Email
    {
        get => email;
        set => SetProperty(ref email, value);
    }

    private int coins;
    public int Coins
    {
        get => coins;
        set
        {
            if (coins != value)
            {
                if(value > coins)
                {
                    _ = animationService.AnimateCoin();
                }
                SetProperty(ref coins, value);
            }
        }
    }

    private User? user;
    public User User
    {
        get => user ?? throw new Exception("User not found");
        set
        {
            _ = apiService.UpdateUserAsync(UpdateUserDto.CreateUpdateUserDto(value));
            user = value;
        }
    }

    public event EventHandler? ThemeChanged;

    public MainViewModel(ApiService apiService, AnimationService animationService, BrowserDetectThemeService browserDetectThemeService, ThemeService themeService)
    {
        this.apiService = apiService;
        this.animationService = animationService;
        this.browserDetectThemeService = browserDetectThemeService;
        this.themeService = themeService;
    }

    public async Task InitializeAsync()
    {
        user = await apiService.GetUserAsync();
        if (user != null)
        {
            var habits = await apiService.GetAllHabitsAsync();
            coins = Habit.GetHabitsEarnings(habits?.ToList() ?? [], DateTime.UtcNow);
            OnPropertyChanged(nameof(Coins));
            Email = User.Email;

            if (User.LightTheme == null)
            {
                User.LightTheme = !await browserDetectThemeService.IsDarkMode();
            }

            themeService.SetTheme(User.LightTheme ?? false ? "material" : "material-dark");
        }
    }

    public void OnThemeChanged()
    {
        ThemeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SidebarClicked()
    {
        if(DefaultSidebarExpanded == false)
        {
            SidebarExpanded = false;
        }
        ViewChanged();
    }

    public void ViewChanged()
    {
    }
}
