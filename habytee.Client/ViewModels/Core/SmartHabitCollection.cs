using System.Collections.ObjectModel;
using System.Collections.Specialized;
using habytee.Client.Services;
using habytee.Interconnection.Models;
using Habytee.Interconnection.Dto;

namespace habytee.Client.ViewModels;

public class SmartHabitCollection : ObservableCollection<Habit>
{
    private readonly IApiService apiService;
    private bool isSyncing;

    public SmartHabitCollection(IApiService apiService)
    {
        this.apiService = apiService;
        CollectionChanged += OnCollectionChanged;
    }

    protected override void InsertItem(int index, Habit item)
    {
        base.InsertItem(index, item);
        if (item.HabitCheckedEvents is ObservableCollection<HabitCheckedEvent> events)
        {
            events.CollectionChanged += (s, e) => OnHabitCheckedEventsChanged(item, e);
        }
    }

    private async void OnHabitCheckedEventsChanged(Habit habit, NotifyCollectionChangedEventArgs e)
    {
        if (isSyncing) return;

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                var habitCheckedEvent = await apiService.CreateHabitCheckedEventAsync(habit.Id);
                if (habitCheckedEvent == null)
                {
                    isSyncing = true;
                    try
                    {
                        if (e.NewStartingIndex < habit.HabitCheckedEvents.Count)
                        {
                            habit.HabitCheckedEvents.RemoveAt(e.NewStartingIndex);
                        }
                    }
                    finally
                    {
                        isSyncing = false;
                    }
                }
                else if (e.NewStartingIndex < habit.HabitCheckedEvents.Count)
                {
                    habit.HabitCheckedEvents[e.NewStartingIndex].Id = habitCheckedEvent.Id;
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems?[0] is HabitCheckedEvent oldEvent)
                {
                    if (!await apiService.DeleteHabitCheckedEventAsync(habit.Id, oldEvent.Id))
                    {
                        isSyncing = true;
                        habit.HabitCheckedEvents.Insert(e.OldStartingIndex, oldEvent);
                        isSyncing = false;
                    }
                }
                break;
        }
    }

    private async void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (isSyncing) return;

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems?[0] is Habit newHabit)
                {
                    var habit = await apiService.CreateHabitAsync(CreateHabitDto.CreateCreateHabitDto(newHabit));
                    if (habit == null)
                    {
                        isSyncing = true;
                        try
                        {
                            Remove(newHabit);
                        }
                        finally
                        {
                            isSyncing = false;
                        }
                    }
                    else
                    {
                        newHabit.Id = habit.Id;
                    }
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems?[0] is Habit oldHabit)
                {
                    if (!await apiService.DeleteHabitAsync(oldHabit.Id))
                    {
                        isSyncing = true;
                        try
                        {
                            Insert(e.OldStartingIndex, oldHabit);
                        }
                        finally
                        {
                            isSyncing = false;
                        }
                    }
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                if (e.NewItems?[0] is Habit updatedHabit)
                {
                    if (!await apiService.UpdateHabitAsync(updatedHabit.Id, updatedHabit))
                    {
                        isSyncing = true;
                        try
                        {
                            this[e.NewStartingIndex] = (Habit)e.OldItems![0]!;
                        }
                        finally
                        {
                            isSyncing = false;
                        }
                    }
                }
                break;
        }
    }

    public async Task Refresh()
    {
        isSyncing = true;
        Clear();
        var habits = await apiService.GetAllHabitsAsync();
        if (habits != null)
        {
            foreach (var habit in habits)
            {
                Add(habit);
            }
        }
        isSyncing = false;
    }
}
