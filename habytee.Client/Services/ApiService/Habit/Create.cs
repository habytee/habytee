using System.Net.Http.Json;
using habytee.Interconnection.Models;
using Habytee.Interconnection.Dto;

namespace habytee.Client.Services;

public partial class ApiService
{
    public async Task<Habit?> CreateHabitAsync(CreateHabitDto habitDto)
    {
        try
        {
            if (habitDto.Alarm.HasValue)
            {
                habitDto.Alarm = DateTime.SpecifyKind(habitDto.Alarm.Value, DateTimeKind.Utc);
            }

            var response = await httpClient.PostAsJsonAsync("/api/habit", habitDto);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error creating habit: {error}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<Habit>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
