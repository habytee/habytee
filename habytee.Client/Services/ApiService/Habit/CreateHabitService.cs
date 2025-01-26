using System.Net.Http.Json;
using habytee.Interconnection.Models;

namespace habytee.Client.Services;

public partial class ApiService
{
    public async Task<bool> CreateHabitAsync(Habit habit)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("/api/habit", habit);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
