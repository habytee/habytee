using habytee.Client.ViewModels;
using Microsoft.AspNetCore.Components;

namespace habytee.Client.Layout;

public partial class MainLayout
{
    [Inject]
    public MainViewModel ViewModel { get; set; } = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        ViewModel.PropertyChanged += (_, _) => StateHasChanged();
    }
}
