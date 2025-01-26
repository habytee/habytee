using habytee.Interconnection.Models;

namespace habytee.Client.Services;

public interface IApiService
{
    Task<List<Habit>?> GetAllHabitsAsync();
    Task<Habit?> GetHabitAsync(int id);
    Task<bool> UpdateHabitAsync(int id, Habit habit);
    Task<bool> DeleteHabitAsync(int id);
    Task<bool> CreateHabitAsync(Habit habit);
    Task<bool> DeleteHabitCheckedEventAsync(int habitId, int checkedEventId);
    Task<bool> CreateHabitCheckedEventAsync(int habitId);
}
