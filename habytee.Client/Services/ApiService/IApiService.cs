using habytee.Interconnection.Models;
using Habytee.Interconnection.Dto;

namespace habytee.Client.Services;

public interface IApiService
{
    Task<List<Habit>?> GetAllHabitsAsync();
    Task<Habit?> GetHabitAsync(int id);
    Task<bool> UpdateHabitAsync(int id, Habit habit);
    Task<bool> DeleteHabitAsync(int id);
    Task<Habit?> CreateHabitAsync(CreateHabitDto habitDto);
    Task<bool> DeleteHabitCheckedEventAsync(int habitId, int checkedEventId);
    Task<HabitCheckedEvent?> CreateHabitCheckedEventAsync(int habitId);
    Task<User?> GetUserAsync();
    Task<User?> UpdateUserAsync(UpdateUserDto user);
}
