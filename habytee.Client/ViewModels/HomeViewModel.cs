using habytee.Client.Model;
using habytee.Client.Services;
using habytee.Interconnection.Models;
using System.Collections.ObjectModel;

namespace habytee.Client.ViewModels;

public class HomeViewModel : BaseViewModel
{
    public event EventHandler? DataLoaded;

    public ObservableCollection<HabitCheckable> YesterdayTasks { get; set; } = new ObservableCollection<HabitCheckable>();
    public ObservableCollection<HabitCheckable> TodayTasks { get; set; } = new ObservableCollection<HabitCheckable>();
    public ObservableCollection<HabitCheckable> TomorrowTasks { get; set; } = new ObservableCollection<HabitCheckable>();
    public ObservableCollection<DayStatistic> DayStatistics { get; set; } = new ObservableCollection<DayStatistic>();
	private IApiService ApiService { get; set; }
    public SmartHabitCollection Habits { get; set; }

    public HomeViewModel(IApiService apiService)
    {
		ApiService = apiService;
        Habits = new SmartHabitCollection(apiService);
        _ = InitializeAsync();
    }

    public async Task InitializeAsync()
    {
        await Habits.Refresh();
        UpdateStats();

        FillTasks(YesterdayTasks, DateTime.Today.AddDays(-1));
        FillTasks(TodayTasks, DateTime.Today);
        FillTasks(TomorrowTasks, DateTime.Today.AddDays(1));
    }


	private void FillTasks(ObservableCollection<HabitCheckable> tasks, DateTime date)
	{
		tasks.Clear();
        foreach (var habit in Habit.GetHabitsToBeDoneOnDay(Habits.ToList(), date))
        {
            tasks.Add(
                new HabitCheckable
                {
                    Habit = habit,
                    IsCompleted = habit.HabitCheckedEvents.Any(hev => hev.TimeStamp == date)
                }
            );
        }
	} 

    public void ToggleHabitCompletion(HabitCheckable habitCheckable)
    {
        Habit? habit = Habits.FirstOrDefault(h => h.Id == habitCheckable.Habit.Id);
        if (habit != null)
        {
            habitCheckable.IsCompleted = !habitCheckable.IsCompleted;
        }
        else
        {
            UpdateStats();

            FillTasks(YesterdayTasks, DateTime.Today.AddDays(-1));
            FillTasks(TodayTasks, DateTime.Today);
            FillTasks(TomorrowTasks, DateTime.Today.AddDays(1));
        }
    }

    public void UpdateStats()
    {
        DayStatistics.Clear();
        foreach (var habit in Habits)
        {
            DayStatistics.Add(
                new DayStatistic
                {
                    Weekday = habit.Weekday.ToString(),
                    CompletionPercentage = Habit.GetHabitsCompletionPercentage(Habits.ToList(), DateTime.Today)
                }
            );
        }
        DataLoaded?.Invoke(this, EventArgs.Empty);
    }
}
