using habytee.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using habytee.Client.Services;
using Radzen.Blazor;

namespace habytee.Client.Pages;

public partial class Home
{
    private HomeViewModel? homeRazorViewModel;
    private RadzenChart? chart;

	[Inject]
	private ApiService? apiService {  get; set; }

    protected override void OnInitialized()
    {
        homeRazorViewModel = new HomeViewModel(apiService!);
        homeRazorViewModel.DataLoaded += (s, e) => 
        {
            chart?.Reload();
            InvokeAsync(StateHasChanged);
        };
    }

    public void Dispose()
    {
        if (homeRazorViewModel != null)
        {
            homeRazorViewModel.DataLoaded -= (s, e) => chart?.Reload();
        }
    }
}
