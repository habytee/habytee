using System.Net.Http.Json;
using habytee.Client.Model;
using habytee.Interconnection.Models;
using habytee.Interconnection.Models.Requests;
using System.Globalization;
using System.Linq;

namespace habytee.Client.Services;

public class ApiService : IApiService
{
    private readonly HttpClient httpClient;
    public ApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<bool> SendHabitToApiAsync(CreateHabitRequest habit)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("/api/habit", habit);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return false;
        }
    }

	public async Task<List<Habit>?> GetHabits(int day)
	{
		try
		{
			var httpResponse = await httpClient.GetAsync($"/api/habit/day/{day}");

			if (!httpResponse.IsSuccessStatusCode)
			{
				Console.WriteLine("statuscode error");
				return null;
			}
			return await httpResponse.Content.ReadFromJsonAsync<List<Habit>>();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return null;
		}
	}

	public async Task<bool> MarkHabitAsChecked(int id)
	{
		try
		{
			var httpResponse = await httpClient.PostAsync($"/api/habit/{id}", null);

			if (!httpResponse.IsSuccessStatusCode)
			{
				Console.WriteLine("statuscode error");
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return false;
		}
	}

    public async Task<List<DayStatistic>> GetStatistics()
    {
        try
        {
            var response = await httpClient.GetAsync("/api/habit/stats");
            if (!response.IsSuccessStatusCode)
            {
                return new List<DayStatistic>();
            }

            var habits = await response.Content.ReadFromJsonAsync<List<Habit>>();
            if (habits == null) return new List<DayStatistic>();

            var statistics = Enumerable.Range(0, 14)
                .Select(i => DateTime.UtcNow.Date.AddDays(-i))
                .Select(date => new DayStatistic
                {
                    Weekday = date.ToString("dd.MM.yyyy"),
                    CompletionPercentage = CalculateCompletionPercentage(habits, date)
                })
                .OrderBy(s => s.Weekday).ToList();

            return statistics;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<DayStatistic>();
        }
    }

    private int CalculateCompletionPercentage(List<Habit> habits, DateTime date)
    {
        var habitsForDay = habits.Where(h => IsHabitActiveOnDate(h, date)).ToList();

        if (!habitsForDay.Any())
		{
			return 100;
		} 

        var completedCount = habitsForDay.Count(h => 
            h.HabitCheckedEvents.Any(e => e.TimeStamp?.Date == date.Date));

        return (int)((double)completedCount / habitsForDay.Count * 100);
    }

    private bool IsHabitActiveOnDate(Habit habit, DateTime date)
    {
        if (habit.CreationDate?.Date > date.Date)
        {
            return false;
        }

        if (date.Date > DateTime.UtcNow.Date)
        {
            return false;
        }

        bool isActive = false;
        if(habit.ABBoth)
        {
            var calendar = CultureInfo.CurrentCulture.Calendar;
            var weekOfYear = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            bool isAWeek = weekOfYear % 2 != 0;

            if(isAWeek)
            {
                if(habit.AWeekDays.Contains(date.DayOfWeek))
                {
                    return true;
                }
            }
            else
            {
                if(habit.BWeekDays.Contains(date.DayOfWeek))
                {
                    return true;
                }
            }
        }
        else
        {
            if(habit.AWeekDays.Contains(date.DayOfWeek))
            {
                return true;
            }
        }
        
        return isActive;
    }
}
