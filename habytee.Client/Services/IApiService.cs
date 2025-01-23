using habytee.Client.Model;
using habytee.Interconnection.Models;
using habytee.Interconnection.Models.Requests;

namespace habytee.Client.Services;

public interface IApiService
{
    Task<bool> SendHabitToApiAsync(CreateHabitRequest habit);
    Task<List<Habit>?> GetHabits(int day);
    Task<bool> MarkHabitAsChecked(int id);
    Task<List<DayStatistic>> GetStatistics();
}
