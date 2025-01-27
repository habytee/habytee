using habytee.Client.Services;
using habytee.Interconnection.Models;
using Radzen;

namespace habytee.Client.ViewModels;

public class HabitsViewModel : BaseViewModel
{
    private readonly ApiService apiService;
    private readonly DialogService dialogService;

    public SmartHabitCollection Habits { get; }

    public HabitsViewModel(ApiService apiService, DialogService dialogService)
    {
        this.apiService = apiService;
        this.dialogService = dialogService;
        Habits = new SmartHabitCollection(apiService);
        _ = Habits.Refresh();
    }

    public async Task DeleteHabitAsync(Habit habit)
    {
        bool? confirmed = await dialogService.Confirm("Are you sure you want to delete this habit?", "Delete Habit?");
        if (confirmed == true)
        {
            Habits.Remove(habit);
        }
    }
}
