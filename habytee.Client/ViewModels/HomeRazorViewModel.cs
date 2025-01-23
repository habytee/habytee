using habytee.Client.Model;
using habytee.Client.Services;
using System.Collections.ObjectModel;

namespace habytee.Client.ViewModels;

public class HomeRazorViewModel : BaseViewModel
{
    public event EventHandler? DataLoaded;

    public ObservableCollection<HabitCheckable> YesterdayTasks { get; set; } = new ObservableCollection<HabitCheckable>();
    public ObservableCollection<HabitCheckable> TodayTasks { get; set; } = new ObservableCollection<HabitCheckable>();
    public ObservableCollection<HabitCheckable> TomorrowTasks { get; set; } = new ObservableCollection<HabitCheckable>();
    public ObservableCollection<DayStatistic> DayStatistics { get; set; } = new ObservableCollection<DayStatistic>();
	private ApiService ApiService { get; set; }

    public HomeRazorViewModel(ApiService apiService)
    {
		ApiService = apiService;

        _ = FillYesterdayTasks();
        _ = FillTodayTasks();
        _ = FillTomorrowTasks();
        _ = FetchStatistics();
    }

	private async Task FillYesterdayTasks()
	{
		var success = await ApiService.GetHabits(-1);
        if(success != null)
        {
            YesterdayTasks.Clear();
            foreach(var habit in success)
            {
                var checkableHabit = new HabitCheckable()
                {
                    Habit = habit,
                    IsCompleted = habit.HabitCheckedEvents.Any(hce => hce.TimeStamp?.Date == DateTime.Today.AddDays(-1))
                };
                YesterdayTasks.Add(checkableHabit);
            }
        }
	} 

    private async Task FillTodayTasks()
	{
		var success = await ApiService.GetHabits(0);
        if(success != null)
        {
            TodayTasks.Clear();
            foreach(var habit in success)
            {
                var checkableHabit = new HabitCheckable()
                {
                    Habit = habit,
                    IsCompleted = habit.HabitCheckedEvents.Any(hce => hce.TimeStamp?.Date == DateTime.Today)
                };
                TodayTasks.Add(checkableHabit);
            }
        }
	} 

    private async Task FillTomorrowTasks()
	{
		var success = await ApiService.GetHabits(1);
        if(success != null)
        {
            TomorrowTasks.Clear();
            foreach(var habit in success)
            {
                var checkableHabit = new HabitCheckable()
                {
                    Habit = habit,
                    IsCompleted = habit.HabitCheckedEvents.Any(hce => hce.TimeStamp?.Date == DateTime.Today.AddDays(1))
                };
                TomorrowTasks.Add(checkableHabit);
            }
        }
	} 

    public async Task ToggleHabitCompletion(HabitCheckable habitCheckable)
    {
        bool success = await ApiService.MarkHabitAsChecked(habitCheckable.Habit.Id);
        if (!success)
        {
            habitCheckable.IsCompleted = !habitCheckable.IsCompleted;
        }
        else
        {
            await FillTodayTasks();
            _ = FetchStatistics();
        }
    }

    public async Task FetchStatistics()
    {
        var statistics = await ApiService.GetStatistics();
        if (statistics != null)
        {
            DayStatistics.Clear();
            foreach (var stat in statistics)
            {
                DayStatistics.Add(stat);
            }
            DataLoaded?.Invoke(this, EventArgs.Empty);
        }
    }
}
