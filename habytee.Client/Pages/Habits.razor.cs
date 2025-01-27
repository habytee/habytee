using habytee.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace habytee.Client.Pages;

public partial class Habits
{
    [Inject]
    private HabitsViewModel habitsViewModel { get; set; } = default!;

    [Inject]
    private DialogService dialogService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await habitsViewModel.Habits.Refresh();
    }
}
