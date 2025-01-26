using habytee.Client.Services;

namespace habytee.Client.ViewModels;

public class HabitsViewModel : BaseViewModel
{
    public SmartHabitCollection Habits { get; }

    public HabitsViewModel(IApiService apiService)
    {
        Habits = new SmartHabitCollection(apiService);
        _ = Habits.Refresh();
    }
}
