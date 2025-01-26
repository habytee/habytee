using System.Net.Http.Json;
using habytee.Interconnection.Models;

namespace habytee.Client.Services;

public partial class ApiService : IApiService
{
    public async Task<Habit?> GetHabitAsync(int id)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<Habit>($"/api/habit/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
