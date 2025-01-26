namespace habytee.Client.ViewModels;

public class MainViewModel : BaseViewModel
{
    private bool sidebarExpanded = true;
    public bool SidebarExpanded 
    { 
        get => sidebarExpanded;
        set => SetProperty(ref sidebarExpanded, value);
    }

    private int coins;
    public int Coins
    {
        get => coins;
        set => SetProperty(ref coins, value);
    }

    public MainViewModel()
    {
        Coins = 0;
    }
}
