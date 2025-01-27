using habytee.Client.Model;
using habytee.Client.Services;
using habytee.Interconnection.Models;
using System.Collections.ObjectModel;

namespace habytee.Client.ViewModels;

public class HomeViewModel : BaseViewModel
{
    public event EventHandler? DataLoaded;

    private readonly SemaphoreSlim toggleSemaphore = new SemaphoreSlim(1, 1);
    private readonly MainViewModel mainViewModel;
    public ObservableCollection<HabitCheckable> YesterdayTasks { get; set; } = new ObservableCollection<HabitCheckable>();
    public ObservableCollection<HabitCheckable> TodayTasks { get; set; } = new ObservableCollection<HabitCheckable>();
    public ObservableCollection<HabitCheckable> TomorrowTasks { get; set; } = new ObservableCollection<HabitCheckable>();
    public ObservableCollection<DayStatistic> DayStatistics { get; set; } = new ObservableCollection<DayStatistic>();
	private ApiService apiService { get; set; }
    public SmartHabitCollection Habits { get; set; }
    private readonly AnimationService animationService;
    public HomeViewModel(ApiService apiService, MainViewModel mainViewModel, AnimationService animationService)
    {
		this.apiService = apiService;
        this.mainViewModel = mainViewModel;
        this.animationService = animationService;
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
                    IsCompleted = habit.HabitCheckedEvents.Any(hev => hev.TimeStamp.Date == date.Date)
                }
            );
        }
	} 

    public async Task ToggleHabitCompletionAsync(HabitCheckable habitCheckable)
    {
        await toggleSemaphore.WaitAsync();
        try
        {
            Habit? habit = Habits.FirstOrDefault(h => h.Id == habitCheckable.Habit.Id);
            if (habit != null)
            {
                if (!habitCheckable.IsCompleted)
                {
                    var eventToRemove = habit.HabitCheckedEvents.FirstOrDefault(e => e.TimeStamp.Date == DateTime.Today.Date);
                    if (eventToRemove != null)
                    {
                        habit.HabitCheckedEvents.Remove(eventToRemove);
                        mainViewModel.Coins = Habit.GetHabitsEarnings(Habits.ToList(), DateTime.Today);
                    }
                }
                else
                {
                    habit.HabitCheckedEvents.Add(new HabitCheckedEvent 
                    { 
                        TimeStamp = DateTime.Today.Date 
                    });
                    mainViewModel.Coins = Habit.GetHabitsEarnings(Habits.ToList(), DateTime.Today);
                }
                
                habitCheckable.IsCompleted = !habitCheckable.IsCompleted;
                UpdateStats();

                FillTasks(YesterdayTasks, DateTime.Today.AddDays(-1));
                FillTasks(TodayTasks, DateTime.Today);
                FillTasks(TomorrowTasks, DateTime.Today.AddDays(1));
            }
        }
        finally
        {
            toggleSemaphore.Release();
        }
    }

    public void UpdateStats()
    {
        DayStatistics.Clear();
        for(int i = 14; i >= 0; i--)
        {
            DayStatistics.Add(
                new DayStatistic
                {
                    Weekday = DateTime.Today.AddDays(-i).ToString(),
                    CompletionPercentage = Habit.GetHabitsCompletionPercentage(Habits.ToList(), DateTime.Today.AddDays(-i))
                }
            );
        }
        DataLoaded?.Invoke(this, EventArgs.Empty);
    }
}
