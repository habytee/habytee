using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;
using habytee.Interconnection.Attributes;
using System.Collections.ObjectModel;

namespace habytee.Interconnection.Models;

public class Habit
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;

    [Required]
    public bool ABBoth { get; set; }

    [Required]
    public List<DayOfWeek> AWeekDays { get; set; } = [];

    [RequiredIf("ABBoth", true)]
    public List<DayOfWeek> BWeekDays { get; set; } = [];

    public DateTime? Alarm { get; set; }

    [Required]
    public int Earnings { get; set; }
    
	public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public int UserId { get; set; } 
    
    [JsonIgnore]
    public User User { get; set; } = null!;
    
	public ObservableCollection<HabitCheckedEvent> HabitCheckedEvents { get; set; } = [];

	public Habit()
    {

    }

    public DayOfWeek Weekday => CreationDate.DayOfWeek;

    public bool IsHabitActiveOnDate(DateTime date)
    {
        if (CreationDate.Date > date.Date)
        {
            return false;
        }

        if (date.Date > DateTime.UtcNow.Date)
        {
            return false;
        }

        if(ABBoth)
        {
            var calendar = CultureInfo.CurrentCulture.Calendar;
            var weekOfYear = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            bool isAWeek = weekOfYear % 2 != 0;

            if(isAWeek)
            {
                if(AWeekDays.Contains(date.DayOfWeek))
                {
                    return true;
                }
            }
            else
            {
                if(BWeekDays.Contains(date.DayOfWeek))
                {
                    return true;
                }
            }
        }
        else
        {
            if(AWeekDays.Contains(date.DayOfWeek))
            {
                return true;
            }
        }
        
        return false;
    }

    public static int GetHabitsDoneOnDay(List<Habit> habits, DateTime date)
    {
        return habits.Where(h => h.HabitCheckedEvents.Any(e => e.TimeStamp?.Date == date.Date)).Count();
    }

    public static List<Habit> GetHabitsToBeDoneOnDay(List<Habit> habits, DateTime date)
    {
        return habits.Where(h => h.IsHabitActiveOnDate(date)).ToList();
    }

    public static int GetHabitsToBeDoneOnDayCount(List<Habit> habits, DateTime date)
    {
        return habits.Where(h => h.IsHabitActiveOnDate(date)).Count();
    }

    public static List<Habit> GetHabitsForDay(List<Habit> habits, DateTime date)
    {
        return habits.Where(h => h.IsHabitActiveOnDate(date)).ToList();
    }

    public static int GetHabitsCompletionPercentage(List<Habit> habits, DateTime date)
    {
        if(habits.Count == 0)
        {
            return 100;
        }

        return (int)(GetHabitsDoneOnDay(habits, date) / GetHabitsToBeDoneOnDayCount(habits, date) * 100);
    }
}
